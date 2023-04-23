import { OtpPartInput } from './opt-part-input';

export interface ResetPasswordResult extends OtpPartInput {
  success: boolean;
  isInvalidCaptcha:boolean;
}
