export interface IFetchResponse<TData> {
    statusCode: number,
    data?: TData,
    messages?: string[]
}