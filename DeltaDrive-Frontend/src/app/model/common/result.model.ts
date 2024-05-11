export interface ErrorResponse {
  message: string;
  metadata: {
    code: number;
    subCode: number;
  };
}

export interface Result<T> {
  valueOrDefault?: T;
  value?: T;
  isFailed: boolean;
  isSuccess: boolean;
  reasons?: Array<ErrorResponse>;
}
