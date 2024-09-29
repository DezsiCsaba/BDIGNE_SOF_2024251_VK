import axios from 'axios'
import { boot } from 'quasar/wrappers'
import createAuthRefreshInterceptor from 'axios-auth-refresh';
import { Notify, Loading } from 'quasar'
import { getErrMsgFromResponse } from "../utils/functions.js";
import {LocalStore} from "../utils/helpers.js";

const isLog = false;
const MAX_RETRIES = 3;

// isLog && console.log('API_URL', process.env.API_URL)
// const api = axios.create({baseURL: process.env.API_URL})

const api = axios.create({baseURL: 'http://localhost:5149/'})

export default boot(({app}) => {
  // for use inside Vue files (Options API) through this.$axios and this.$api

  app.config.globalProperties.$axios = axios
  // ^ ^ ^ this will allow you to use this.$axios (for Vue Options API form)
  //       so you won't necessarily have to import axios in each vue file

  app.config.globalProperties.$api = api
  // ^ ^ ^ this will allow you to use this.$api (for Vue Options API form)
  //       so you can easily perform requests against your app's API
})

api.interceptors.request.use(config => {
  isLog && console.log('api-------config', config);
  config.headers['Authorization'] = `Bearer ${getAccessToken()}`;
  if (config.isLoading !== false) {
    // Loading.show({
    //   delay: 200 // ms
    // })
  }
  return config;
}, error => {
  // handle the error
  return Promise.reject(error);
});

api.interceptors.response.use(
  async response => {
    Loading.hide()
    if (response?.data?.error) {
      let caption = '-'
      try {
        caption = JSON.stringify(response?.data?.error)
      } catch (e) {
        /* empty */
      }
      Notify.create({
        type: 'my-notif',
        icon: 'contactless',
        message: `Hiba történt.`,
        caption,
        color: 'accent',
        progress: true,
        timeout: 8500,
        html: true
      })
      console.warn('Hiba történt:', caption);
      LocalStore.storeErrMsg(response?.data?.error)
    }
    return response
  },
  async error => {
    const config = error.config
    if (!config.__retryCount) {
      config.__retryCount = 0;
    }
    Loading.hide()
    // console.error({error, e:error.response});
    if (!error.response) {
      Notify.create({
        type: 'my-notif',
        icon: 'contactless',
        message: `Please check your internet connection.`,
        caption: "Couldn't connect to the server..",
        color: 'grey-8',
        progress: true,
        timeout: 5000,
        html: true
      })
      return retryIfAllowed(config)
    }
    if (error?.response?.data?.error) {
      const msg = getErrMsgFromResponse(error.response) || error?.response?.data?.error
      Notify.create({
        type: 'my-notif',
        icon: 'warning',
        message: `Hiba történt.`,
        caption: msg,
        color: 'accent',
        progress: true,
        timeout: 8500,
        html: true
      })
      LocalStore.storeErrMsg(error?.response?.data?.error)
      return Promise.resolve(error?.response)
    }

    isLog && console.log('error?.response?.status', error?.response?.status);
    if (error?.response?.status === 500) {
      const {url, method} = error.response.config
      Notify.create({
        type: 'my-notif',
        icon: 'contactless',
        message: `Szerver hiba történt.`,
        caption: `infó <small>(${method}: ${url})</small>\``,
        color: 'red-8',
        progress: true,
        timeout: 8500,
        html: true
      })
      LocalStore.storeErrMsg(error.response?.data)
    } else if ([401, 403].includes(error?.response?.status) && error?.response?.config?.url === '/api/token/refresh') {
      //TODO - auto logout store-ból
      setTimeout(() => {
        console.warn('redirect to /');
        window.location.href = '/' // history.go()
      }, 1000)
    }
    return Promise.reject(error)
  }
)


// Function that will be called to refresh authorization
const refreshAuthLogic = failedRequest => {
  if (failedRequest.response.status === 401) {
    const {url, method} = failedRequest.response.config
    isLog && console.log('failedRequest.response.status 401', {url, method});
    Notify.create({
      type: 'my-notif',
      icon: 'contactless',
      message: `A kérés hitelesítése nem sikerült. <small style="color: silver">(${method}: ${url})</small>`,
      caption: 'Kérem jelentkezzen be újra.',
      color: 'grey-8',
      progress: true,
      timeout: 8500,
      html: true
    })
    return Promise.reject()
  }
  return new Error('nincs lefejlesztve a logic') //TODO - lekérni egy új tokent a szervertől
}

// Instantiate the interceptor (you can chain it as it returns the axios instance)
createAuthRefreshInterceptor(api, refreshAuthLogic, {
  statusCodes: [401, 403], // default: [ 401 ]
  retryInstance: api,
  pauseInstanceWhileRefreshing: true
});

// Obtain the fresh token each time the function is called
function getAccessToken() {
  return localStorage.getItem('token');
}

// Use interceptor to inject the token to requests
axios.interceptors.request.use(request => {
  request.headers['Authorization'] = `Bearer ${getAccessToken()}`;
  return request;
});

const retryIfAllowed = async (config) => {
  if (config.__retryCount < MAX_RETRIES) {
    config.__retryCount += 1;
    console.log(`Retrying request... (${config.__retryCount} out of ${MAX_RETRIES})`);
    Notify.create({
      type: 'my-notif',
      icon: 'contactless',
      message: `Retrying..`,
      color: 'info',
      progress: true,
      timeout: 2000,
      html: true
    })
    await new Promise(resolve => setTimeout(resolve, 2000));
    return api(config);
  }
}

export { axios, api }
