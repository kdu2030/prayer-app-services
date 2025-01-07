﻿using Microsoft.EntityFrameworkCore;
using PrayerAppServices.Data;
using PrayerAppServices.PrayerGroups.Entities;
using PrayerAppServices.PrayerGroups.Models;

namespace PrayerAppServices.PrayerGroups {
    public class PrayerGroupRepository(AppDbContext dbContext) : IPrayerGroupRepository {
        private readonly AppDbContext _dbContext = dbContext;

        public CreatePrayerGroupResponse CreatePrayerGroup(string adminUsername, NewPrayerGroup newPrayerGroup) {
            FormattableString sqlQuery = $@"SELECT * FROM create_prayer_group(
                {adminUsername},
                {newPrayerGroup.Name},
                {newPrayerGroup.Description},
                {newPrayerGroup.Rules},
                {newPrayerGroup.Color},
                {newPrayerGroup.ImageFileId}
            )";


            return _dbContext.Database.SqlQuery<CreatePrayerGroupResponse>(
               sqlQuery
            ).First();
        }
    }
}
