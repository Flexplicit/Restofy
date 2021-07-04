import axios, { AxiosRequestConfig } from 'axios'
import { ApiBaseUrl } from '../configurations'
import { IFetchResponse } from '../types/IFetchResponse'
/* eslint-disable */ // this thing is making me crazy

export abstract class BaseServices {
  // constructor(protected ApiEndPointUrl: string, private jwt?: string | null) {}

  protected static getAxiosConfiguration(jwt?: string | null): AxiosRequestConfig | undefined {
    if (jwt === undefined || jwt === null) {
      return undefined
    }

    const config: AxiosRequestConfig = {
      headers: {
        Authorization: 'Bearer ' + jwt,
      },
    }
    return config
  }

  protected static axios = axios.create({
    baseURL: ApiBaseUrl,
    validateStatus: function (status) {
      return status < 500
    },
    headers: {
      'Content-Type': 'application/json',
    },
  })

  static async getAll<TEntity>(apiEndPoint: string, jwt?: string | null, id?: string): Promise<IFetchResponse<TEntity[]>> {
    const url = id === undefined ? apiEndPoint : apiEndPoint + '/' + id
    console.log(BaseServices.getAxiosConfiguration(jwt)?.headers)

    try {
      const res = await this.axios.get<TEntity[]>(url, { headers: BaseServices.getAxiosConfiguration(jwt)?.headers })
      return {
        statusCode: res.status,
        data: res.data,
        messages: [res.statusText],
      }
    } catch (error) {
      return { statusCode: 0, messages: [(error as Error).message] }
    }
  }
  static async addObject<TEntity>(object: TEntity | TEntity[], apiEndPoint: string, jwt?: string | null, id?: string): Promise<IFetchResponse<TEntity | TEntity[]>> {
    const url = id === undefined ? apiEndPoint : apiEndPoint + '/' + id
    console.log(BaseServices.getAxiosConfiguration(jwt)?.headers)
    debugger
    try {
      const res = await this.axios.post<TEntity>(url, object, { headers: BaseServices.getAxiosConfiguration(jwt)?.headers })
      // if (res.status >= 200 && res.status < 300) {
      return {
        statusCode: res.status,
        data: res.data as TEntity | TEntity[],
        messages: [res.statusText],

        // }
        // } else {
        // return { statusCode: res.status, messages: Object.values(res.data.errors).flatMap((x) => x) as string[] }
      }
    } catch (error) {
      console.log((error as Error).message)
      return { statusCode: 0, messages: [(error as Error).message] }
    }
  }

  static async getObjectById<TEntity>(apiEndPoint: string, id: string, jwt?: string | null): Promise<IFetchResponse<TEntity>> {
    const url = apiEndPoint + '/' + id
    try {
      const res = await this.axios.get<TEntity>(url, {
        headers: BaseServices.getAxiosConfiguration(jwt)?.headers,
      })
      return {
        statusCode: res.status,
        data: res.data as TEntity,
        messages: [res.statusText],
      }
    } catch (error) {
      return { statusCode: 0, messages: [(error as Error).message] }
    }
  }

  static async updateAnObject<TEntity>(updatedObject: TEntity, apiEndPoint: string, id: string, jwt?: string | null): Promise<IFetchResponse<TEntity>> {
    const url = apiEndPoint + '/' + id
    try {
      const res = await this.axios.put<TEntity>(url, updatedObject, {
        headers: BaseServices.getAxiosConfiguration(jwt)?.headers,
      })
      return {
        statusCode: res.status,
        data: res.data as TEntity,
        messages: [res.statusText],
      }
    } catch (error) {
      return { statusCode: 0, messages: [(error as Error).message] }
    }
  }

  static async deleteObjectById<TEntity>(apiEndPoint: string, id: string, jwt?: string | null): Promise<IFetchResponse<TEntity>> {
    const url = apiEndPoint + '/' + id
    try {
      const res = await this.axios.delete<TEntity>(url, {
        headers: BaseServices.getAxiosConfiguration(jwt)?.headers,
      })
      return {
        statusCode: res.status,
        data: res.data as TEntity,
        messages: [res.statusText],
      }
    } catch (error) {
      return { statusCode: 0, messages: [(error as Error).message] }
    }
  }
}
