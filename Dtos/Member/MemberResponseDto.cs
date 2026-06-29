using mvc_dotnet.Enums;

namespace mvc_dotnet.Dtos.Member;

public class MemberResponseDto
{
    public Guid MemberId {get;set;}
    public Guid ProjectId {get;set;}
    public Guid UserId {get;set;}
    public MemberRole Role {get;set;}     public DateTime JoinedAt {get;set;}
    public string FirstName {get;set;} = string.Empty;
    public string LastName {get;set;} = string.Empty;
    
}