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
            : this( new ResidencyContext() )
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ResidencyService{T}"/> class.
        /// </summary>
        /// <param name="residencyContext">The residency context.</param>
        public ResidencyService( ResidencyContext residencyContext )
            : base( residencyContext)
        {
            ResidencyContext = residencyContext;
        }

        /// <summary>
        /// Gets the residency context.
        /// </summary>
        /// <value>
        /// The residency context.
        /// </value>
        public ResidencyContext ResidencyContext { get; private set; }

        /// <summary>
        /// Determines whether this instance can delete the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="errorMessage">The error message.</param>
        /// <returns>
        ///   <c>true</c> if this instance can delete the specified item; otherwise, <c>false</c>.
        /// </returns>
        public bool CanDelete( CompetencyPersonProjectAssessment item, out string errorMessage )
        {
            errorMessage = string.Empty;

            if ( new ResidencyService<CompetencyPersonProjectAssessmentPointOfAssessment>().Queryable().Any( a => a.CompetencyPersonProjectAssessmentId == item.Id ) )
            {
                errorMessage = string.Format( "This {0} is assigned to a {1}.", CompetencyPersonProjectAssessment.FriendlyTypeName, CompetencyPersonProjectAssessmentPointOfAssessment.FriendlyTypeName );
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
        public bool CanDelete( CompetencyPersonProject item, out string errorMessage )
        {
            errorMessage = string.Empty;

            if ( new ResidencyService<CompetencyPersonProjectAssessment>().Queryable().Any( a => a.CompetencyPersonProjectId == item.Id ) )
            {
                errorMessage = string.Format( "This {0} is assigned to a {1}.", CompetencyPersonProject.FriendlyTypeName, CompetencyPersonProjectAssessment.FriendlyTypeName );
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
        public bool CanDelete( CompetencyPerson item, out string errorMessage )
        {
            errorMessage = string.Empty;

            if ( new ResidencyService<CompetencyPersonProject>().Queryable().Any( a => a.CompetencyPersonId == item.Id ) )
            {
                errorMessage = string.Format( "This {0} is assigned to a {1}.", CompetencyPerson.FriendlyTypeName, CompetencyPersonProject.FriendlyTypeName );
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
        public bool CanDelete( Competency item, out string errorMessage )
        {
            errorMessage = string.Empty;

            if ( new ResidencyService<CompetencyPerson>().Queryable().Any( a => a.CompetencyId == item.Id ) )
            {
                errorMessage = string.Format( "This {0} is assigned to a {1}.", Competency.FriendlyTypeName, CompetencyPerson.FriendlyTypeName );
                return false;
            }

            if ( new ResidencyService<Project>().Queryable().Any( a => a.CompetencyId == item.Id ) )
            {
                errorMessage = string.Format( "This {0} is assigned to a {1}.", Competency.FriendlyTypeName, Project.FriendlyTypeName );
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
        public bool CanDelete( Period item, out string errorMessage )
        {
            errorMessage = string.Empty;

            if ( new ResidencyService<Track>().Queryable().Any( a => a.PeriodId == item.Id ) )
            {
                errorMessage = string.Format( "This {0} is assigned to a {1}.", Period.FriendlyTypeName, Track.FriendlyTypeName );
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
        public bool CanDelete( ProjectPointOfAssessment item, out string errorMessage )
        {
            errorMessage = string.Empty;

            if ( new ResidencyService<CompetencyPersonProjectAssessmentPointOfAssessment>().Queryable().Any( a => a.ProjectPointOfAssessmentId == item.Id ) )
            {
                errorMessage = string.Format( "This {0} is assigned to a {1}.", ProjectPointOfAssessment.FriendlyTypeName, CompetencyPersonProjectAssessmentPointOfAssessment.FriendlyTypeName );
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
        public bool CanDelete( Project item, out string errorMessage )
        {
            errorMessage = string.Empty;

            if ( new ResidencyService<CompetencyPersonProject>().Queryable().Any( a => a.ProjectId == item.Id ) )
            {
                errorMessage = string.Format( "This {0} is assigned to a {1}.", Project.FriendlyTypeName, CompetencyPersonProject.FriendlyTypeName );
                return false;
            }

            if ( new ResidencyService<ProjectPointOfAssessment>().Queryable().Any( a => a.ProjectId == item.Id ) )
            {
                errorMessage = string.Format( "This {0} is assigned to a {1}.", Project.FriendlyTypeName, ProjectPointOfAssessment.FriendlyTypeName );
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
        public bool CanDelete( Track item, out string errorMessage )
        {
            errorMessage = string.Empty;

            if ( new ResidencyService<Competency>().Queryable().Any( a => a.TrackId == item.Id ) )
            {
                errorMessage = string.Format( "This {0} is assigned to a {1}.", Track.FriendlyTypeName, Competency.FriendlyTypeName );
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
        public bool CanDelete( CompetencyPersonProjectAssessmentPointOfAssessment item, out string errorMessage )
        {
            errorMessage = string.Empty;
            return true;
        }
    }
}
