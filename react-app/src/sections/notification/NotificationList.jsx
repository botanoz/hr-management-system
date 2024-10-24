import React, { useState, useEffect, useCallback } from 'react';
import {
  List,
  ListItem,
  ListItemAvatar,
  ListItemText,
  Avatar,
  Typography,
  IconButton,
  Button,
  Box,
} from '@mui/material';
import { Delete as DeleteIcon, Notifications as NotificationsIcon } from '@mui/icons-material';
import { getNotifications, markNotificationAsRead, deleteNotification } from '../../services/api';
import useNotification from '../../hooks/useNotification';
import { fToNow } from '../../utils/formatTime';

const NotificationList = () => {
  const [notifications, setNotifications] = useState([]);
  const { addNotification } = useNotification();

  const fetchNotifications = useCallback(async () => {
    try {
      const response = await getNotifications();
      setNotifications(response.data);
    } catch (error) {
      addNotification({ type: 'error', message: 'Failed to fetch notifications' });
    }
  }, [addNotification]);

  useEffect(() => {
    fetchNotifications();
  }, [fetchNotifications]);

  const handleMarkAsRead = async (id) => {
    try {
      await markNotificationAsRead(id);
      setNotifications((prevNotifications) =>
        prevNotifications.map((notif) =>
          notif.id === id ? { ...notif, isRead: true } : notif
        )
      );
      addNotification({ type: 'success', message: 'Notification marked as read' });
    } catch (error) {
      addNotification({ type: 'error', message: 'Failed to mark notification as read' });
    }
  };

  const handleDelete = async (id) => {
    try {
      await deleteNotification(id);
      setNotifications((prevNotifications) =>
        prevNotifications.filter((notif) => notif.id !== id)
      );
      addNotification({ type: 'success', message: 'Notification deleted' });
    } catch (error) {
      addNotification({ type: 'error', message: 'Failed to delete notification' });
    }
  };

  const handleMarkAllAsRead = async () => {
    try {
      await Promise.all(
        notifications.filter((n) => !n.isRead).map((n) => markNotificationAsRead(n.id))
      );
      setNotifications((prevNotifications) =>
        prevNotifications.map((notif) => ({ ...notif, isRead: true }))
      );
      addNotification({ type: 'success', message: 'All notifications marked as read' });
    } catch (error) {
      addNotification({ type: 'error', message: 'Failed to mark all notifications as read' });
    }
  };

  return (
    <Box>
      <Box display="flex" justifyContent="space-between" alignItems="center" mb={2}>
        <Typography variant="h4">Notifications</Typography>
        <Button onClick={handleMarkAllAsRead} disabled={!notifications.some((n) => !n.isRead)}>
          Mark All as Read
        </Button>
      </Box>
      <List>
        {notifications.length === 0 ? (
          <Typography align="center">No notifications</Typography>
        ) : (
          notifications.map((notification) => (
            <ListItem
              key={notification.id}
              secondaryAction={
                <IconButton edge="end" aria-label="delete" onClick={() => handleDelete(notification.id)}>
                  <DeleteIcon />
                </IconButton>
              }
              sx={{ bgcolor: notification.isRead ? 'transparent' : 'action.hover' }}
            >
              <ListItemAvatar>
                <Avatar>
                  <NotificationsIcon />
                </Avatar>
              </ListItemAvatar>
              <ListItemText
                primary={notification.message}
                secondary={
                  <>
                    <Typography component="span" variant="body2" color="text.primary">
                      {fToNow(notification.createdAt)}
                    </Typography>
                    {!notification.isRead && (
                      <Button size="small" onClick={() => handleMarkAsRead(notification.id)} sx={{ ml: 1 }}>
                        Mark as Read
                      </Button>
                    )}
                  </>
                }
              />
            </ListItem>
          ))
        )}
      </List>
    </Box>
  );
};

export default NotificationList;
