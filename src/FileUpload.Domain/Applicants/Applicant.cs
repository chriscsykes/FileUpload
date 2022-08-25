using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;
using JetBrains.Annotations;

using Volo.Abp;

namespace FileUpload.Applicants
{
    public class Applicant : FullAuditedAggregateRoot<Guid>
    {
        [CanBeNull]
        public virtual string FirstName { get; set; }

        [CanBeNull]
        public virtual string LastName { get; set; }

        [CanBeNull]
        public virtual string Email { get; set; }

        public Applicant()
        {

        }

        public Applicant(Guid id, string firstName, string lastName, string email)
        {

            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
        }

    }
}