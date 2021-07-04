import axios, { AxiosRequestConfig } from 'axios'
import { IFetchResponse } from '../types/IFetchResponse'
/* eslint-disable */ // this thing is making me crazy

export class BaseServices<TEntity> {
  constructor(protected ApiEndPointUrl: string) {}

  protected getAxiosConfiguration(jwt?: string | null): AxiosRequestConfig | undefined {
    if (jwt === undefined || jwt === null) {
      return undefined;
    }

    const config: AxiosRequestConfig = {
      headers: {
        Authorization: 'Bearer ' + jwt,
      },
    }
    return config
  }

  // async getAll(id?: string): Promise<IFetchResponse<TEntity[]>> {

  //     const url = id === undefined ? this.ApiEndPointUrl : this.ApiEndPointUrl + "/" + id
  //     try {
  //         const res = await axios.get(url, { headers: this.authHeaders })
  //         if (res.status === 200) {
  //             return {
  //                 statusCode: res.status,
  //                 data: res.data as TEntity[]
  //             }
  //         } else {
  //             //  errors: Object.values(res.data.errors).flatMap(x => x).concat((res.data.messages !== undefined) ? res.data.messages : [])
  //             return { statusCode: res.status, messages: [res.statusText] }
  //         }

  //     } catch (error) {
  //         console.log((error as Error).message)
  //         return { statusCode: 0, messages: [(error as Error).message] }
  //     }
  // }
  // async addObject(object: TEntity | TEntity[]): Promise<IFetchResponse<TEntity | TEntity[]>> {
  //     // console.log(object)
  //     try {
  //         const res = await axios.post(
  //             this.ApiEndPointUrl,
  //             object,
  //             {
  //                 validateStatus: (status) => status < 500,
  //                 headers: this.authHeaders
  //             }
  //         )

  //         if (res.status >= 200 && res.status < 300) {
  //             // console.log("data is", res);
  //             return {
  //                 statusCode: res.status,
  //                 data: res.data as TEntity | TEntity[]
  //             }
  //         } else {
  //             return { statusCode: res.status, messages: Object.values(res.data.errors).flatMap(x => x) as string[] }
  //         }
  //     } catch (error) {
  //         console.log((error as Error).message)
  //         return { statusCode: 0, messages: [(error as Error).message] }
  //     }
  // }

  async getObjectById(id: string, jwt?: string | null): Promise<IFetchResponse<TEntity>> {
    const url = this.ApiEndPointUrl + '/' + id
    console.log(this.getAxiosConfiguration(jwt))
    try {
      const res = await axios.get(url, {
        headers: this.getAxiosConfiguration(jwt)?.headers,
        validateStatus: (status) => status < 500,
      })
      if (res.status === 200) {
        return {
          statusCode: res.status,
          data: res.data as TEntity,
        }
      } else {
        return { statusCode: res.status, messages: [res.statusText] }
      }
    } catch (error) {
      console.log((error as Error).message)
      return { statusCode: 0, messages: [(error as Error).message] }
    }
  }

  // async updateAnObject(updatedObject: TEntity, id: string): Promise<IFetchResponse<TEntity>> {
  //     const url = this.ApiEndPointUrl + "/" + id;
  //     console.log(updatedObject, "updated object")
  //     try {
  //         const res = await axios.put(url, updatedObject, {
  //             headers: this.authHeaders, validateStatus: (status) => status < 500
  //         })
  //         if (res.status === 200) {
  //             return {
  //                 statusCode: res.status,
  //                 data: res.data as TEntity
  //             }
  //         } else {
  //             return { statusCode: res.status, messages: [res.statusText] }
  //         }

  //     } catch (error) {
  //         console.log((error as Error).message)
  //         return { statusCode: 0, messages: [(error as Error).message] }
  //     }
  // }
  // async deleteAnObjectById(id: string): Promise<IFetchResponse<TEntity>> {
  //     const url = this.ApiEndPointUrl + "/" + id
  //     try {
  //         const res = await axios.delete(url, {
  //             headers: this.authHeaders, validateStatus: (status) => status < 500
  //         })
  //         if (res.status === 204) {
  //             return {
  //                 statusCode: res.status,
  //             }
  //         } else {
  //             return { statusCode: res.status, messages: [res.statusText] }
  //         }

  //     } catch (error) {
  //         console.log((error as Error).message)
  //         return { statusCode: 0, messages: [(error as Error).message] }
  //     }
  // }
}
