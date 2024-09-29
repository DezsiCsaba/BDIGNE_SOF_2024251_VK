import { defineStore } from 'pinia'
import { api } from 'boot/axios'
import {LocalStore} from "../utils/helpers.js";


export const useAuthStore = defineStore('auth', {
  state: () => ({
    token: '',
    userData: {},
  }),
  actions: {
    async auth(payload) {
      console.log(payload)
      const res = await api({
        method: 'post',
        url: 'auth',
        data: payload
      })
      this.token = res.data.token
      this.userData = res.data.userData
      LocalStore.storeToken(this.token)
      LocalStore.storeUserData(this.userData)
    },
  },
})
