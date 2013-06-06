using System.Linq;
using com.ccvonline.Residency.Model;
using Rock.Data;

namespace com.ccvonline.Residency.Data
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ResidencyService<T> : Rock.Data.Service<T> where T : Rock.Data.Entity<T>, new()
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ResidencyService{T}"/> class.
        /// </summary>
        public ResidencyService()
            : base( new EFRepository<T>( new ResidencyContext() ) )
        {
        }

        /// <summary>
        /// Determines whether this instance can delete the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="errorMessage">The error message.</param>
        /// <returns>
        ///   <c>true</c> if this instance can delete the specified item; otherwise, <c>false</c>.
        /// </returns>
        public bool CanDelete( ResidencyCompetencyPersonProjectAssignmentAssessment item, out string errorMessage )
        {
            errorMessage = string.Empty;

            if ( new ResidencyService<ResidencyCompetencyPersonProjectAssignmentAssessmentPointOfAssessment>().Queryable().Any( a => a.ResidencyCompetencyPersonProjectAssignmentAssessmentId == item.Id ) )
            {
                errorMessage = string.Format( "This {0} is assigned to a {1}.", ResidencyCompetencyPersonProjectAssignmentAssessment.FriendlyTypeName, ResidencyCompetencyPersonProjectAssignmentAssessmentPointOfAssessment.FriendlyTypeName );
                return false;
            }

            return true;
        }

        /// <summary>
        /// Determines whether this instance can delete the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="errorMessage">The error message.</param>
        /// <returns>
        ///   <c>true</c> if this instance can delete the specified item; otherwise, <c>false</c>.
        /// </returns>
        public bool CanDelete( ResidencyCompetencyPersonProjectAssignment item, out string errorMessage )
        {
            errorMessage = string.Empty;

            if ( new ResidencyService<ResidencyCompetencyPersonProjectAssignmentAssessment>().Queryable().Any( a => a.ResidencyCompetencyPersonProjectAssignmentId == item.Id ) )
            {
                errorMessage = string.Format( "This {0} is assigned to a {1}.", ResidencyCompetencyPersonProjectAssignment.FriendlyTypeName, ResidencyCompetencyPersonProjectAssignmentAssessment.FriendlyTypeName );
                return false;
            }

            return true;
        }

        /// <summary>
        /// Determines whether this instance can delete the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="errorMessage">The error message.</param>
        /// <returns>
        ///   <c>true</c> if this instance can delete the specified item; otherwise, <c>false</c>.
        /// </returns>
        public bool CanDelete( ResidencyCompetencyPersonProject item, out string errorMessage )
        {
            errorMessage = string.Empty;

            if ( new ResidencyService<ResidencyCompetencyPersonProjectAssignment>().Queryable().Any( a => a.ResidencyCompetencyPersonProjectId == item.Id ) )
            {
                errorMessage = string.Format( "This {0} is assigned to a {1}.", ResidencyCompetencyPersonProject.FriendlyTypeName, ResidencyCompetencyPersonProjectAssignment.FriendlyTypeName );
                return false;
            }

            return true;
        }

        /// <summary>
        /// Determines whether this instance can delete the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="errorMessage">The error message.</param>
        /// <returns>
        ///   <c>true</c> if this instance can delete the specified item; otherwise, <c>false</c>.
        /// </returns>
        public bool CanDelete( ResidencyCompetencyPerson item, out string errorMessage )
        {
            errorMessage = string.Empty;

            if ( new ResidencyService<ResidencyCompetencyPersonProject>().Queryable().Any( a => a.ResidencyCompetencyPersonId == item.Id ) )
            {
                errorMessage = string.Format( "This {0} is assigned to a {1}.", ResidencyCompetencyPerson.FriendlyTypeName, ResidencyCompetencyPersonProject.FriendlyTypeName );
                return false;
            }

            return true;
        }

        /// <summary>
        /// Determines whether this instance can delete the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="errorMessage">The error message.</param>
        /// <returns>
        ///   <c>true</c> if this instance can delete the specified item; otherwise, <c>false</c>.
        /// </returns>
        public bool CanDelete( ResidencyCompetency item, out string errorMessage )
        {
            errorMessage = string.Empty;

            if ( new ResidencyService<ResidencyCompetencyPerson>().Queryable().Any( a => a.ResidencyCompetencyId == item.Id ) )
            {
                errorMessage = string.Format( "This {0} is assigned to a {1}.", ResidencyCompetency.FriendlyTypeName, ResidencyCompetencyPerson.FriendlyTypeName );
                return false;
            }

            if ( new ResidencyService<ResidencyProject>().Queryable().Any( a => a.ResidencyCompetencyId == item.Id ) )
            {
                errorMessage = string.Format( "This {0} is assigned to a {1}.", ResidencyCompetency.FriendlyTypeName, ResidencyProject.FriendlyTypeName );
                return false;
            }

            return true;
        }

        /// <summary>
        /// Determines whether this instance can delete the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="errorMessage">The error message.</param>
        /// <returns>
        ///   <c>true</c> if this instance can delete the specified item; otherwise, <c>false</c>.
        /// </returns>
        public bool CanDelete( ResidencyPeriod item, out string errorMessage )
        {
            errorMessage = string.Empty;

            if ( new ResidencyService<ResidencyTrack>().Queryable().Any( a => a.ResidencyPeriodId == item.Id ) )
            {
                errorMessage = string.Format( "This {0} is assigned to a {1}.", ResidencyPeriod.FriendlyTypeName, ResidencyTrack.FriendlyTypeName );
                return false;
            }

            return true;
        }

        /// <summary>
        /// Determines whether this instance can delete the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="errorMessage">The error message.</param>
        /// <returns>
        ///   <c>true</c> if this instance can delete the specified item; otherwise, <c>false</c>.
        /// </returns>
        public bool CanDelete( ResidencyProjectPointOfAssessment item, out string errorMessage )
        {
            errorMessage = string.Empty;

            if ( new ResidencyService<ResidencyCompetencyPersonProjectAssignmentAssessmentPointOfAssessment>().Queryable().Any( a => a.ResidencyProjectPointOfAssessmentId == item.Id ) )
            {
                errorMessage = string.Format( "This {0} is assigned to a {1}.", ResidencyProjectPointOfAssessment.FriendlyTypeName, ResidencyCompetencyPersonProjectAssignmentAssessmentPointOfAssessment.FriendlyTypeName );
                return false;
            }

            return true;
        }

        /// <summary>
        /// Determines whether this instance can delete the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="errorMessage">The error message.</param>
        /// <returns>
        ///   <c>true</c> if this instance can delete the specified item; otherwise, <c>false</c>.
        /// </returns>
        public bool CanDelete( ResidencyProject item, out string errorMessage )
        {
            errorMessage = string.Empty;

            if ( new ResidencyService<ResidencyCompetencyPersonProject>().Queryable().Any( a => a.ResidencyProjectId == item.Id ) )
            {
                errorMessage = string.Format( "This {0} is assigned to a {1}.", ResidencyProject.FriendlyTypeName, ResidencyCompetencyPersonProject.FriendlyTypeName );
                return false;
            }

            if ( new ResidencyService<ResidencyProjectPointOfAssessment>().Queryable().Any( a => a.ResidencyProjectId == item.Id ) )
            {
                errorMessage = string.Format( "This {0} is assigned to a {1}.", ResidencyProject.FriendlyTypeName, ResidencyProjectPointOfAssessment.FriendlyTypeName );
                return false;
            }

            return true;
        }

        /// <summary>
        /// Determines whether this instance can delete the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="errorMessage">The error message.</param>
        /// <returns>
        ///   <c>true</c> if this instance can delete the specified item; otherwise, <c>false</c>.
        /// </returns>
        public bool CanDelete( ResidencyTrack item, out string errorMessage )
        {
            errorMessage = string.Empty;

            if ( new ResidencyService<ResidencyCompetency>().Queryable().Any( a => a.ResidencyTrackId == item.Id ) )
            {
                errorMessage = string.Format( "This {0} is assigned to a {1}.", ResidencyTrack.FriendlyTypeName, ResidencyCompetency.FriendlyTypeName );
                return false;
            }

            return true;
        }
    }
}
