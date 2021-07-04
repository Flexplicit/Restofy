import axios, { AxiosInstance } from 'axios'
import { data } from 'jquery'
import { IUserLogin } from '../types/IAccountTypes/IUserLogin'
import { IUserRegister } from '../types/IAccountTypes/IUserRegister'
import { IFetchResponse } from '../types/IFetchResponse'
import { IJwtResponse } from '../types/IJwtResponse'
import { IResponseMessage } from '../types/IResponseMessage'

export class AccountService {
  constructor(protected apiEndPointUrl: string) {}

  createAxiosClient(apiEndPoint: string): AxiosInstance {
    const axiosApiClient = axios.create({
      baseURL: apiEndPoint,
      validateStatus: function (status) {
        return status < 500
      },
    })
    return axiosApiClient
  }

  async login(userLogin: IUserLogin): Promise<IFetchResponse<IJwtResponse | IResponseMessage>> {
    const url = this.apiEndPointUrl + '/Login'
    try {
      const res = await axios.post(url, userLogin, { validateStatus: (status) => status < 500 })
      if (res.status === 200) {
        return {
          statusCode: res.status,
          data: res.data as IJwtResponse,
        }
      }
      return {
        statusCode: res.status,
        data: {
          errors: res.data.messages,
        } as IResponseMessage,
      }
    } catch (error) {
      console.log((error as Error).name, 'login faILED')
    }
    return {
      statusCode: 0,
      messages: ['Something bad happened'],
      data: {} as IResponseMessage,
    }
  }

  async register(userRegister: IUserRegister): Promise<IFetchResponse<IJwtResponse | IResponseMessage>> {
    const url = this.apiEndPointUrl + '/Register'
    try {
      const res = await axios.post(url, userRegister, { validateStatus: (status) => status < 500 })
      if (res.status === 200) {
        return {
          statusCode: res.status,
          data: res.data as IJwtResponse,
        }
      }
      const errors = Object.values(res.data.errors !== undefined ? res.data.errors : []) as string[]

      return {
        statusCode: res.status,
        data: {
          // transfering modelstate object array errors into array and joining them with custom errors
          errors: Object.values(res.data.errors !== undefined ? res.data.errors : [])
            .flatMap((x) => x)
            .concat(res.data.messages !== undefined ? res.data.messages : []),
        } as IResponseMessage,
      }
    } catch (error) {
      console.log((error as Error).name, 'login faILED')
    }
    return {
      statusCode: 0,
      messages: ['Something bad happened'],
      data: {} as IResponseMessage,
    }
  }
}
