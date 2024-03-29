﻿using RabbitMQChallenge.Tracking.Application.Models;

namespace RabbitMQChallenge.Tracking.Application.Services
{
    public interface ILocationService
    {
        /// <summary>
        /// Performs validations for location update request
        /// </summary>
        /// <param name="locationUpdate">request</param>
        /// <returns></returns>
        bool IsValidUpdate(LocationUpdateRequest locationUpdate);

        /// <summary>
        /// Process the request and send message to the bus
        /// </summary>
        /// <param name="updateRequest"></param>
        void ProcessUpdate(LocationUpdateRequest updateRequest);
    }
}
