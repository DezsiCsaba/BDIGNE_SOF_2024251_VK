import { defineStore } from 'pinia'
import { api } from 'boot/axios'


export const useTicketStore = defineStore('ticket', {
  state: () => ({
    list: [],
    currentTicket: {}
  }),
  actions: {
    async load() {
      const res = await api({
        method: 'get',
        url: 'ticket/get'
      })
      this.list = res.data.tickets
    },

    async getTicketsWithAsigneeId(payload) {
      const res = await api({
        method: 'get',
        url: `ticket/get/asignee/${payload.userId}`
      })
      this.list = res.data.tickets
    },

    async getTicketById(payload){
      const res = await api({
        method: 'get',
        url: `ticket/${payload.ticketId}`
      })
      // this.currentTicket = res.data.ticket
      return res.data.ticket
    },

    async getTicketsByProjectId(payload){
      const res = await api({
        method: 'get',
        url: `ticket/get/project/${payload.projectId}`
      })
      this.list = res.data.tickets
    },

    async updateTicket(payload){
      await api({
        method: 'put',
        url: 'ticket/update',
        data: payload
      })
    },
  },
})
