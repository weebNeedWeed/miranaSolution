namespace miranaSolution.DTOs.Systems.Mails;

public record EmailRequest(
    string Receiver,
    string Subject,
    string Body);