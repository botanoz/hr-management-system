import React from 'react';
import { Snackbar, Alert } from '@mui/material';
import useNotification from '../../hooks/useNotification';

const Notification = () => {
  const { notifications, removeNotification } = useNotification();

  const handleClose = (event, reason, id) => {
    if (reason === 'clickaway') {
      return;
    }
    removeNotification(id);
  };

  return (
    <>
      {notifications.map((notification) => (
        <Snackbar
          key={notification.id}
          open
          autoHideDuration={6000}
          onClose={(event, reason) => handleClose(event, reason, notification.id)}
          anchorOrigin={{ vertical: 'bottom', horizontal: 'right' }}
        >
          <Alert
            onClose={(event) => handleClose(event, 'closeButton', notification.id)}
            severity={notification.type}
            sx={{ width: '100%' }}
          >
            {notification.message}
          </Alert>
        </Snackbar>
      ))}
    </>
  );
};

export default Notification;