namespace compliments_complaints_service.Mappers
{
    public static class CouncilDepartmentSubMapper
    {
        public static string SetComplaintCouncilDepartmentSub(string revsBensDept, string environmentDept, string planningDept)
        {
            var councilDepartmentSub = string.Empty;

            if (!string.IsNullOrEmpty(revsBensDept))
            {
                councilDepartmentSub = revsBensDept;
            }

            if (!string.IsNullOrEmpty(environmentDept))
            {
                councilDepartmentSub = environmentDept;
            }

            if (!string.IsNullOrEmpty(planningDept))
            {
                councilDepartmentSub = planningDept;
            }

            return councilDepartmentSub;
        }
    }
}
