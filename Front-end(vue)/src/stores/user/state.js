export default function () {
  return {
    user: JSON.parse(localStorage.getItem('user')) || null,
    token: localStorage.getItem('jwt') || null
  }
}
