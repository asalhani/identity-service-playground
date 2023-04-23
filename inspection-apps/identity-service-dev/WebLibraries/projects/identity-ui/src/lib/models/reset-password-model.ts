export interface ResetPasswordModel {
  emailToken: string;
  password: string;
  captchaString: string;
}
