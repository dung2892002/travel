export default function () {
  return {
    user: JSON.parse(localStorage.getItem('user')) || null,
    token: localStorage.getItem('jwt') || null,
    users: null,
    totalPages: 1,
    totalItems: 1,
    wallet: null,
    wallets: null
  }
}
