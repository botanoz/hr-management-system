import io from 'socket.io-client';

const SOCKET_URL = process.env.REACT_APP_SOCKET_URL || 'http://localhost:3000';

let socket;

export const initSocket = () => {
  socket = io(SOCKET_URL, {
    transports: ['websocket'],
  });

  socket.on('connect', () => {
    console.log('Socket connected');
  });

  socket.on('disconnect', () => {
    console.log('Socket disconnected');
  });

  return socket;
};

export const getSocket = () => {
  if (!socket) {
    throw new Error('Socket not initialized. Call initSocket first.');
  }
  return socket;
};

export const closeSocket = () => {
  if (socket) {
    socket.close();
  }
};

export const subscribeToNotifications = (callback) => {
  if (!socket) {
    throw new Error('Socket not initialized. Call initSocket first.');
  }
  socket.on('notification', callback);
};

export const unsubscribeFromNotifications = (callback) => {
  if (!socket) {
    throw new Error('Socket not initialized. Call initSocket first.');
  }
  socket.off('notification', callback);
};

export default {
  initSocket,
  getSocket,
  closeSocket,
  subscribeToNotifications,
  unsubscribeFromNotifications,
};