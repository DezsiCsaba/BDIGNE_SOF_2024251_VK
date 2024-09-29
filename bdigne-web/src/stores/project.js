import { defineStore } from 'pinia'
import { api } from 'boot/axios'


export const useProjectStore = defineStore('project', {
  state: () => ({
    projects: [],
    currentProject: null
  }),
  actions: {
    async getAll() {
      const res = await api({
        method: 'get',
        url: 'project'
      })
      this.projects = res.data.projects
    },

    async getById(payload){
      const res = await api({
        method: 'get',
        url: `project/${payload.projectId}`
      })
      this.currentProject = res.data.project
    },
  },
})
