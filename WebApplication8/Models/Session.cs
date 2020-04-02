using System;
using System.Collections.Generic;
using System.Text;

namespace WebApplication8.Models
{
    public class Session
    {
        public string Duration { get; set; }   //  duration              
        public string DatabaseName { get; set; }   //  database_name         
        public string SessionId { get; set; }   //  session_id            
        public string HostName { get; set; }   //  host_name             
        public string LoginName { get; set; }   //  login_name            
        public string BlockingSessionId { get; set; }   //  blocking_session_id   
        public string BlockedSessionCount { get; set; }   //  blocked_session_count 
        public string WaitInfo { get; set; }   //  wait_info             
        public string ProgramName { get; set; }   //  program_name          
        public string CPU { get; set; }   //  CPU                   
        public string Reads { get; set; }   //  reads                 
        public string PhysicalReads { get; set; }   //  physical_reads        
        public string Writes { get; set; }   //  writes                
        public string SqlCommand { get; set; }   //  sql_command           
        public string SqlText { get; set; }   //  sql_text              
        public string QueryPlan { get; set; }   //  query_plan            
        public string Status { get; set; }   //  status                
        public string PercentComplete { get; set; }   //  percent_complete      
    }

}
