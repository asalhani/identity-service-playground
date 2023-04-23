export interface ChangeExpiredPasswordResult {
  success: boolean;
  isInvalidCaptcha: boolean;
  isInvalidRequest: boolean;
  isInvalidNewPassword: boolean;
  invalidPasswordIssues: string;
}
