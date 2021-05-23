using GemSto.Common.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace GemSto.Domain.User
{
    public class UserAudit
    {
        public int Id { get; private set; }

        [Required, StringLength(200)]
        public string UserId { get; private set; }

        [Required]
        public DateTimeOffset Timestamp { get; private set; } = DateTime.UtcNow;

        [Required]
        public UserAuditEventType AuditEvent { get; set; }
        [StringLength(200)]
        public string IpAddress { get; private set; }

        public static UserAudit CreateAuditEvent(string userId, UserAuditEventType auditEventType, string ipAddress)
        {
            return new UserAudit { UserId = userId, AuditEvent = auditEventType, IpAddress = ipAddress };
        }
    }
}
