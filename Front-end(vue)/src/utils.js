export function formatDateForm(date) {
  const d = new Date(date)
  let month = '' + (d.getMonth() + 1)
  let day = '' + d.getDate()
  const year = d.getFullYear()

  if (month.length < 2) month = '0' + month
  if (day.length < 2) day = '0' + day

  return [year, month, day].join('-')
}

export function formatDate(inputDate) {
  const date = new Date(inputDate)
  const day = String(date.getDate()).padStart(2, '0')
  const month = String(date.getMonth() + 1).padStart(2, '0')
  const year = date.getFullYear()
  return `${day}/${month}/${year}`
}

export function formatNumber(value) {
  return Intl.NumberFormat('de-DE').format(value)
}

export function getLinkImage(path) {
  const imageServer = import.meta.env.VITE_IMAGE_HOST
  return `${imageServer}/${path}?alt=media`
}

export function calculateDay(start, end) {
  const checkInDate = new Date(start)
  const checkOutDate = new Date(end)
  const timeDifference = checkOutDate - checkInDate // Tính sự chênh lệch thời gian
  const daysDifference = timeDifference / (1000 * 3600 * 24) // Chuyển đổi từ mili giây sang ngày
  return daysDifference
}
