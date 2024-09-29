import { defineStore } from 'pinia'
import { api } from 'boot/axios'

export const useTestStore = defineStore('test', {
  state: () => ({
    token: '',
  }),
  actions: {
    async getToken() {
      const res = await api({
        method: 'get',
        url: 'dummy/auth'
      })
      this.token = res.data.token
    },
  },
})
