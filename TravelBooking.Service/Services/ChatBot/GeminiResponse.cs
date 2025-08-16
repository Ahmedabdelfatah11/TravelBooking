using Google.Cloud.AIPlatform.V1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelBooking.Service.Services.ChatBot
{
    public class GeminiResponse
    {
        public Candidate[] Candidates { get; set; }
    }
}
