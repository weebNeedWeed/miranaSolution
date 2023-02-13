import axios from 'axios';

class BaseApiClient {
  axiosInstance;

  constructor(headersObj = {}) {
    this.axiosInstance = axios.create({
      baseURL: process.env.REACT_APP_BASE_ADDRESS,
      timeout: 1000,
      headers: headersObj,
    });
  }
}

export default BaseApiClient;
