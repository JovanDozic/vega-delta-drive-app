export interface PagedResult<T> {
  results: T[];
  totalCount: number;
  page: number;
  pageSize: number;
}
