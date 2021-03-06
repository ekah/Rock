﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using com.ccvonline.Residency.Model;

namespace com.ccvonline.Residency.Data
{
    /// <summary>
    /// 
    /// </summary>
    public partial class ResidencyContext : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ResidencyContext"/> class.
        /// </summary>
        public ResidencyContext()
            : base( "RockContext" )
        {
            // intentionally left blank
        }

        /// <summary>
        /// This method is called when the model for a derived context has been initialized, but
        /// before the model has been locked down and used to initialize the context.  The default
        /// implementation of this method does nothing, but it can be overridden in a derived class
        /// such that the model can be further configured before it is locked down.
        /// </summary>
        /// <param name="modelBuilder">The builder that defines the model for the context being created.</param>
        /// <remarks>
        /// Typically, this method is called only once when the first instance of a derived context
        /// is created.  The model for that context is then cached and is for all further instances of
        /// the context in the app domain.  This caching can be disabled by setting the ModelCaching
        /// property on the given ModelBuidler, but note that this can seriously degrade performance.
        /// More control over caching is provided through use of the DbModelBuilder and DbContextFactory
        /// classes directly.
        /// </remarks>
        protected override void OnModelCreating( DbModelBuilder modelBuilder )
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            // Model Configurations
            modelBuilder.Configurations.Add( new CompetencyConfiguration() );
            modelBuilder.Configurations.Add( new CompetencyPersonConfiguration() );
            modelBuilder.Configurations.Add( new CompetencyPersonProjectConfiguration());
            modelBuilder.Configurations.Add( new CompetencyPersonProjectAssessmentConfiguration());
            modelBuilder.Configurations.Add( new CompetencyPersonProjectAssessmentPointOfAssessmentConfiguration());
            modelBuilder.Configurations.Add( new PeriodConfiguration() );
            modelBuilder.Configurations.Add( new ProjectConfiguration());
            modelBuilder.Configurations.Add( new ProjectPointOfAssessmentConfiguration());
            modelBuilder.Configurations.Add( new TrackConfiguration() );
        }
        
        #region Models

        /// <summary>
        /// Gets or sets the residency competencies.
        /// </summary>
        /// <value>
        /// The residency competencies.
        /// </value>
        public DbSet<Competency> ResidencyCompetencies { get; set; }

        /// <summary>
        /// Gets or sets the residency competency persons.
        /// </summary>
        /// <value>
        /// The residency competency persons.
        /// </value>
        public DbSet<CompetencyPerson> CompetencyPersons { get; set; }

        /// <summary>
        /// Gets or sets the residency competency person projects.
        /// </summary>
        /// <value>
        /// The residency competency person projects.
        /// </value>
        public DbSet<CompetencyPersonProject> CompetencyPersonProjects { get; set; }

        /// <summary>
        /// Gets or sets the competency person project assessments.
        /// </summary>
        /// <value>
        /// The competency person project assessments.
        /// </value>
        public DbSet<CompetencyPersonProjectAssessment> CompetencyPersonProjectAssessments { get; set; }

        /// <summary>
        /// Gets or sets the competency person project assessment point of assessments.
        /// </summary>
        /// <value>
        /// The competency person project assessment point of assessments.
        /// </value>
        public DbSet<CompetencyPersonProjectAssessmentPointOfAssessment> CompetencyPersonProjectAssessmentPointOfAssessments { get; set; }
        
        /// <summary>
        /// Gets or sets the residency periods.
        /// </summary>
        /// <value>
        /// The residency periods.
        /// </value>
        public DbSet<Period> Periods { get; set; }

        /// <summary>
        /// Gets or sets the residency projects.
        /// </summary>
        /// <value>
        /// The residency projects.
        /// </value>
        public DbSet<Project> Projects { get; set; }

        /// <summary>
        /// Gets or sets the residency project point of assessments.
        /// </summary>
        /// <value>
        /// The residency project point of assessments.
        /// </value>
        public DbSet<ProjectPointOfAssessment> ProjectPointOfAssessments { get; set; }

        /// <summary>
        /// Gets or sets the residency tracks.
        /// </summary>
        /// <value>
        /// The residency tracks.
        /// </value>
        public DbSet<Track> Tracks { get; set; }
        
        #endregion
    }
}
