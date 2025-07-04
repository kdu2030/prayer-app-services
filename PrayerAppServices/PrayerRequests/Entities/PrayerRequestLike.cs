﻿using PrayerAppServices.Users.Entities;
using System.ComponentModel.DataAnnotations;

namespace PrayerAppServices.PrayerRequests.Entities {
    public class PrayerRequestLike {
        public int? Id { get; set; }

        [Required]
        public PrayerRequest? PrayerRequest { get; set; }

        [Required]
        public AppUser? User { get; set; }
    }
}
