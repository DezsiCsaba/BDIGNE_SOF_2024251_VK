export function toBool(val) {
  try{
    return !!JSON.parse(val)
  }catch (e) {
    return false
  }
}

export function getErrMsgFromResponse(res){
  if (res?.data?.error?.msg){
    return res.data.error.msg
  }
  else if (res?.data?.error?.message){
    return res.data.error.message
  }
}

export function lowerCaseFirstLetter(val){
  if (val !== null) return val.charAt(0).toLowerCase() + val.slice(1);
  return ''
}

export function upperFirstLetter(val){
  if (val !== null) return val.charAt(0).toUpperCase() + val.slice(1);
  return ''
}

export function toReadableDate(timeStampString){
  const date = new Date(timeStampString)
  //this is the 'default' for this function
  // return date.toLocaleDateString('en-UK', {
  //   year: 'numeric', month: 'long', day: 'numeric', weekday: 'long'
  // })
  const options = {
    year: 'numeric',   // "2024"
    month: 'long',     // "September"
    day: 'numeric',    // "13"
    weekday: 'long',   // "Friday"
  }
  const year = date.toLocaleDateString('en-US', { year: 'numeric' })
  const month = date.toLocaleDateString('en-US', { month: 'long' })
  const day = date.toLocaleDateString('en-US', { day: 'numeric' })
  const weekday = date.toLocaleDateString('en-US', { weekday: 'long' })

  return `${year}, ${month} ${day}, (${weekday})`
}

export function getCurrentUrl() {
  return window.location.href
}


const ticketColorMap = {
  Open: 'negative text-bold text-uppercase',
  Done: 'positive',
  InProgress: 'warning',
  Closed: 'primary',

  Low: 'positive',
  Medium: 'warning',
  High: 'negative'
}
export const ticketStatusAndPriorityColorfn = (row='', col='', val='') => {
  // console.log('ticketStatusAndPriorityColorfn, val', val)
  if (row || col){
    return ticketColorMap[row[col.name]]
  }
  return ticketColorMap[val]
}

const priorityIconMap = {
  Low: 'keyboard_double_arrow_down',
  Medium: 'menu',
  High: 'keyboard_double_arrow_up'
}
export const priorityToIcon = (row='', col='', val='') => {
  // console.log('priorityToIcon, val', val)
  if (row || col){
    return priorityIconMap[row[col.name]]
  }
  return priorityIconMap[val]
}

const activityEventTemplateMap = {
  default: "<div>%user% made changes - %date%<br>%type% - <b>Original: </b>%original%, <b>New: </b>%new%</div>",
  ticketUpdate: "<div>%user% made changes - %date%<br>%type% - <b>Original: </b>%original%, <b>New: </b>%new%</div>"
}
export function getActivityEventTemplate(type = 'default'){
  return activityEventTemplateMap[type]
}


