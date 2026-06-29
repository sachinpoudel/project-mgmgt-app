using mvc_dotnet.Enums;

namespace mvc_dotnet.Dtos.Member;

public class CreateMemberDto
{
    public string LastName {get;set;} = string.Empty;
    public string FirstName {get;set;} = string.Empty;
    public Guid MemberId {get;set;}
    public Guid ProjectId {get;set;}
    public Guid UserId {get;set;}
    public MemberRole Role {get;set;} // default role is viewer, can be changed to contributor or owner as needed
   
}