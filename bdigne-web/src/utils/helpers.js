export const TokenStorage = {
  LOCAL_STORAGE_ERR_MSG: 'errMsg',
  LOCAL_STORAGE_TOKEN: 'token',
}


export class LocalStore {

  static getErrMsg() {
    try {
      const str = localStorage.getItem(TokenStorage.LOCAL_STORAGE_ERR_MSG)
      return str && JSON.parse(str) || []
    }
    catch (err){
      return null
    }
  }

  static storeErrMsg(errMsg) {
    const list = LocalStore.getErrMsg() || []
    localStorage.setItem(TokenStorage.LOCAL_STORAGE_ERR_MSG, JSON.stringify([...list, { date: new Date().toISOString(), errMsg }]))
  }

  static storeToken(token){
    localStorage.setItem(TokenStorage.LOCAL_STORAGE_TOKEN, token)
  }

  static storeUserData(payload) {
    localStorage.setItem('userData', JSON.stringify(payload))
  }

  static getUserData() {
    return JSON.parse(localStorage.getItem('userData'))
  }
}
