﻿using System;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Configuration;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Configuration;

using Rock.Attribute;
using Rock.Model;
using Rock.Security;
using Rock.Web.UI;

namespace com.ccvonline.Authentication
{
    /// <summary>
    /// Authenticates a username/password using Arena's password encryption
    /// </summary>
    [Description( "Arena Authentication Provider" )]
    [Export( typeof( AuthenticationComponent ) )]
    [ExportMetadata( "ComponentName", "Arena" )]
    [BooleanField("Convert To Database Login", "Should the Arena user be converted to use the Rock Database service after first succesful authentication?")]
    public class Arena : AuthenticationComponent
    {
        private static byte[] encryptionKey;

        /// <summary>
        /// Initializes the <see cref="Arena" /> class.
        /// </summary>
        /// <exception cref="System.Configuration.ConfigurationErrorsException">Authentication requires a 'PasswordKey' app setting</exception>
        static Arena()
        {
            var passwordKey = ConfigurationManager.AppSettings["PasswordKey"];
            if ( String.IsNullOrWhiteSpace( passwordKey ) )
            {
                throw new ConfigurationErrorsException( "Authentication requires a 'PasswordKey' app setting" );
            }

            encryptionKey = HexToByte( passwordKey );
        }

        /// <summary>
        /// Authenticates the specified user name.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="password">The password.</param>
        /// <returns></returns>
        public override bool Authenticate( UserLogin user, string password )
        {
            byte[] bytes = ConvertToByteArray( user.Password );

            string encodedPassword = EncodePassword( user, password );
            bool valid = encodedPassword == Convert.ToBase64String( bytes );

            bool convert = false;
            if ( valid && Boolean.TryParse( GetAttributeValue( "ConvertToDatabaseLogin" ), out convert ) && convert )
            {
                // Convert to database type
                var service = new UserLoginService();
                var rockUser = service.GetByUserName( user.UserName );
                if ( rockUser != null )
                {
                    rockUser.Password = DatabaseEncodePassword( password, encryptionKey );
                    rockUser.ServiceName = "Rock.Security.Authentication.Database";
                    service.Save( rockUser, null );
                }
            }

            return valid;
        }

        /// <summary>
        /// Encodes the password.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="password"></param>
        /// <returns></returns>
        public override String EncodePassword( UserLogin user, string password )
        {
            SHA1 hash = SHA1.Create();
            return Convert.ToBase64String( hash.ComputeHash( Encoding.Unicode.GetBytes( password ) ) );
        }

        private string DatabaseEncodePassword( string password, byte[] encryptionKey )
        {
            HMACSHA1 hash = new HMACSHA1();
            hash.Key = encryptionKey;
            return Convert.ToBase64String( hash.ComputeHash( Encoding.Unicode.GetBytes( password ) ) );
        }

        private static byte[] ConvertToByteArray( string value )
        {
            byte[] bytes = null;
            if ( String.IsNullOrEmpty( value ) )
                bytes = new byte[0];
            else
            {
                int string_length = value.Length;
                int character_index = ( value.StartsWith( "0x", StringComparison.Ordinal ) ) ? 2 : 0; // Does the string define leading HEX indicator '0x'. Adjust starting index accordingly.               
                int number_of_characters = string_length - character_index;

                bool add_leading_zero = false;
                if ( 0 != ( number_of_characters % 2 ) )
                {
                    add_leading_zero = true;

                    number_of_characters += 1;  // Leading '0' has been striped from the string presentation.
                }

                bytes = new byte[number_of_characters / 2]; // Initialize our byte array to hold the converted string.

                int write_index = 0;
                if ( add_leading_zero )
                {
                    bytes[write_index++] = FromCharacterToByte( value[character_index], character_index );
                    character_index += 1;
                }

                for ( int read_index = character_index; read_index < value.Length; read_index += 2 )
                {
                    byte upper = FromCharacterToByte( value[read_index], read_index, 4 );
                    byte lower = FromCharacterToByte( value[read_index + 1], read_index + 1 );

                    bytes[write_index++] = (byte)( upper | lower );
                }
            }

            return bytes;
        }

        private static byte FromCharacterToByte( char character, int index, int shift = 0 )
        {
            byte value = (byte)character;
            if ( ( ( 0x40 < value ) && ( 0x47 > value ) ) || ( ( 0x60 < value ) && ( 0x67 > value ) ) )
            {
                if ( 0x40 == ( 0x40 & value ) )
                {
                    if ( 0x20 == ( 0x20 & value ) )
                        value = (byte)( ( ( value + 0xA ) - 0x61 ) << shift );
                    else
                        value = (byte)( ( ( value + 0xA ) - 0x41 ) << shift );
                }
            }
            else if ( ( 0x29 < value ) && ( 0x40 > value ) )
                value = (byte)( ( value - 0x30 ) << shift );
            else
                throw new InvalidOperationException( String.Format( "Character '{0}' at index '{1}' is not valid alphanumeric character.", character, index ) );

            return value;
        }

        private static byte[] HexToByte( string hexString )
        {
            byte[] returnBytes = new byte[hexString.Length / 2];
            for ( int i = 0; i < returnBytes.Length; i++ )
                returnBytes[i] = Convert.ToByte( hexString.Substring( i * 2, 2 ), 16 );
            return returnBytes;
        }

    }
}