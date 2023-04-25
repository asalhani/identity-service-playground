export interface UserLogoutResult {
  success: boolean;
  isShowPrompt: boolean;
  postLogoutRedirectUri: string;
  clientName: string;
}
