
export interface BaseUserLoginInput{
  loginName: string;
}
export interface PublicUserLoginModel extends BaseUserLoginInput{
  loginName: string;
}

export interface EmployeeUserLoginModel extends BaseUserLoginInput{
  password: string;
}
