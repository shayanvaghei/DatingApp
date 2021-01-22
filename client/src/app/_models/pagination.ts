export interface Pagination {
    currentPage: number;
    itemsPerPage: number;
    totalItems: number;
    totalPages: number;
}

//T will be an array of member
export class PaginatedResult<T> {
    result: T;
    pagination: Pagination;
}