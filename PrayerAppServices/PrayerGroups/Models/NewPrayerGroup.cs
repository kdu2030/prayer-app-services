﻿namespace PrayerAppServices.PrayerGroups.Models {
    public class NewPrayerGroup {
        public required string Name { get; set; }
        public string? Description { get; set; }
        public string? Rules { get; set; }
        public int? Color { get; set; }
        public int? ImageFileId { get; set; }
    }
}