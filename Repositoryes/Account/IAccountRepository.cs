﻿using BeautySaloon.Models;
using Microsoft.AspNetCore.Mvc;

namespace BeautySaloon.Repositoryes.Account
{
    public interface IAccountRepository
    {
        public Task<List<Appointment>> GetAppointmentsByUserIdAsync(int userId);
        Task<User> FindUserByIdAsync(int userId);
        Task UpdateUserAsync(User user);
        Task DeleteAppointmentAsync(int appointmentId);
        Task<List<Appointment>> LoadMoreAppointments(int skip, int userId, int take = 10 );


	}
}
