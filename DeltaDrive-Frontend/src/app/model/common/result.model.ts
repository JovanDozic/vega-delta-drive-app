export interface Result<T> {
  valueOrDefault?: T;
  value?: T;
  isFailed: boolean;
  isSuccess: boolean;
  reasons?: Array<any>;
}
