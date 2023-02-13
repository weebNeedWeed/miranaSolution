import BaseApiClient from './BaseApiClient';

class SlidesApiClient extends BaseApiClient {
  async getAll() {
    try {
      const response = await this.axiosInstance.get('/slides');

      return response.data.data;
    } catch (ex) {
      return null;
    }
  }
}

export default new SlidesApiClient();
