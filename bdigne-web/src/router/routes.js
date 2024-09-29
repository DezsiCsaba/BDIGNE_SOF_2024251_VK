const routes = [
  {
    path: '/',
    component: () => import('layouts/MainLayout.vue'),
    children: [
      { path: '', component: () => import('pages/IndexPage.vue') },
      { path: '/dash', component: () => import('pages/DashBoard.vue') },
      { path: '/ticket/:id', component: () => import('pages/Ticket.vue') },
      { path: '/project', component: () => import('pages/Project.vue') },
      { path: '/project/:id', component: () => import('pages/Project.vue') },
    ]
  },

  // Always leave this as last one,
  // but you can also remove it
  {
    path: '/:catchAll(.*)*',
    component: () => import('pages/ErrorNotFound.vue')
  }
]

export default routes
