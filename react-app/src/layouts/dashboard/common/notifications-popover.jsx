import React, { useState, useEffect, useCallback } from 'react';
import PropTypes from 'prop-types';
import Box from '@mui/material/Box';
import List from '@mui/material/List';
import Badge from '@mui/material/Badge';
import Button from '@mui/material/Button';
import Avatar from '@mui/material/Avatar';
import Divider from '@mui/material/Divider';
import Tooltip from '@mui/material/Tooltip';
import Popover from '@mui/material/Popover';
import Typography from '@mui/material/Typography';
import IconButton from '@mui/material/IconButton';
import ListItemText from '@mui/material/ListItemText';
import ListSubheader from '@mui/material/ListSubheader';
import ListItemAvatar from '@mui/material/ListItemAvatar';
import ListItemButton from '@mui/material/ListItemButton';

import NotificationsIcon from '@mui/icons-material/Notifications';
import DoneAllIcon from '@mui/icons-material/DoneAll';
import InfoIcon from '@mui/icons-material/Info';
import WarningIcon from '@mui/icons-material/Warning';
import ErrorIcon from '@mui/icons-material/Error';
import AvatarIcon from '@mui/icons-material/Person';
import ClockIcon from '@mui/icons-material/AccessTime';

import { fToNow } from 'src/utils/format-time';
import Scrollbar from 'src/components/scrollbar';
import { getNotifications, updateNotification } from 'src/services/api';

export default function NotificationsPopover() {
  const [notifications, setNotifications] = useState([]);
  const [open, setOpen] = useState(null);

  const fetchNotifications = useCallback(async () => {
    try {
      const response = await getNotifications();
      setNotifications(response.data || []);
    } catch (error) {
      console.error('Failed to fetch notifications:', error);
    }
  }, []);

  useEffect(() => {
    fetchNotifications();
  }, [fetchNotifications]);

  const totalUnRead = notifications.filter((item) => !item.isRead).length;

  const handleMarkAllAsRead = useCallback(async () => {
    const unreadNotifications = notifications.filter((notification) => !notification.isRead);

    await Promise.all(
      unreadNotifications.map(async (notification) => {
        try {
          await updateNotification(notification.id, {
            ...notification,
            dateRead: new Date().toISOString(),
          });
        } catch (error) {
          console.error(`Failed to update notification with id ${notification.id}:`, error);
        }
      })
    );

    setNotifications((prevNotifications) =>
      prevNotifications.map((notification) => ({
        ...notification,
        isRead: true,
        dateRead: new Date().toISOString(),
      }))
    );
  }, [notifications]);

  const handleOpen = useCallback((event) => {
    setOpen(event.currentTarget);

    setTimeout(() => {
      handleMarkAllAsRead();
    }, 15000);
  }, [handleMarkAllAsRead]);

  const handleClose = () => {
    setOpen(null);
  };

  const handleNotificationClick = async (id) => {
    const notificationToUpdate = notifications.find((n) => n.id === id);

    if (notificationToUpdate && !notificationToUpdate.isRead) {
      try {
        await updateNotification(id, {
          ...notificationToUpdate,
          dateRead: new Date().toISOString(),
        });

        setNotifications((prevNotifications) =>
          prevNotifications.map((notification) =>
            notification.id === id
              ? { ...notification, isRead: true, dateRead: new Date().toISOString() }
              : notification
          )
        );
      } catch (error) {
        console.error(`Failed to update notification with id ${id}:`, error);
      }
    }
  };

  return (
    <>
      <IconButton color={open ? 'primary' : 'default'} onClick={handleOpen}>
        <Badge badgeContent={totalUnRead} color="error">
          <NotificationsIcon width={24} />
        </Badge>
      </IconButton>

      <Popover
        open={!!open}
        anchorEl={open}
        onClose={handleClose}
        anchorOrigin={{ vertical: 'bottom', horizontal: 'right' }}
        transformOrigin={{ vertical: 'top', horizontal: 'right' }}
        PaperProps={{
          sx: {
            mt: 1.5,
            ml: 0.75,
            width: 360,
          },
        }}
      >
        <Box sx={{ display: 'flex', alignItems: 'center', py: 2, px: 2.5 }}>
          <Box sx={{ flexGrow: 1 }}>
            <Typography variant="subtitle1">Notifications</Typography>
            <Typography variant="body2" sx={{ color: 'text.secondary' }}>
              You have {totalUnRead} unread messages
            </Typography>
          </Box>

          {totalUnRead > 0 && (
            <Tooltip title="Mark all as read">
              <IconButton color="primary" onClick={handleMarkAllAsRead}>
                <DoneAllIcon />
              </IconButton>
            </Tooltip>
          )}
        </Box>

        <Divider sx={{ borderStyle: 'dashed' }} />

        <Scrollbar sx={{ height: { xs: 340, sm: 'auto' } }}>
          <List
            disablePadding
            subheader={
              <ListSubheader disableSticky sx={{ py: 1, px: 2.5, typography: 'overline' }}>
                New
              </ListSubheader>
            }
          >
            {notifications.slice(0, 2).map((notification) => (
              <NotificationItem
                key={notification.id}
                notification={notification}
                onClick={() => handleNotificationClick(notification.id)}
              />
            ))}
          </List>

          <List
            disablePadding
            subheader={
              <ListSubheader disableSticky sx={{ py: 1, px: 2.5, typography: 'overline' }}>
                Earlier
              </ListSubheader>
            }
          >
            {notifications.slice(2).map((notification) => (
              <NotificationItem
                key={notification.id}
                notification={notification}
                onClick={() => handleNotificationClick(notification.id)}
              />
            ))}
          </List>
        </Scrollbar>

        <Divider sx={{ borderStyle: 'dashed' }} />

        <Box sx={{ p: 1 }}>
          <Button fullWidth disableRipple>
            View All
          </Button>
        </Box>
      </Popover>
    </>
  );
}

// ----------------------------------------------------------------------

NotificationItem.propTypes = {
  notification: PropTypes.shape({
    id: PropTypes.number.isRequired,
    isRead: PropTypes.bool.isRequired,
    message: PropTypes.string.isRequired,
    notificationType: PropTypes.string.isRequired,
    dateSent: PropTypes.string.isRequired,
  }).isRequired,
  onClick: PropTypes.func.isRequired,
};

function NotificationItem({ notification, onClick }) {
  const { avatar, title } = renderContent(notification);

  const createdAtDate = notification.dateSent ? new Date(notification.dateSent) : null;
  const formattedTime = createdAtDate && !Number.isNaN(createdAtDate.getTime())
    ? fToNow(createdAtDate)
    : "Invalid date";

  return (
    <ListItemButton
      sx={{
        py: 1.5,
        px: 2.5,
        mt: '1px',
        ...(notification.isRead === false && {
          bgcolor: 'action.selected',
        }),
      }}
      onClick={onClick}
    >
      <ListItemAvatar>
        <Avatar sx={{ bgcolor: 'background.neutral' }}>{avatar}</Avatar>
      </ListItemAvatar>
      <ListItemText
        primary={title}
        secondary={
          <Typography
            variant="caption"
            sx={{
              mt: 0.5,
              display: 'flex',
              alignItems: 'center',
              color: 'text.disabled',
            }}
          >
            <ClockIcon sx={{ mr: 0.5, width: 16, height: 16 }} />
            {formattedTime}
          </Typography>
        }
      />
    </ListItemButton>
  );
}

// ----------------------------------------------------------------------

function renderContent(notification) {
  const title = (
    <Typography variant="subtitle2">
      {notification.message}
      <Typography component="span" variant="body2" sx={{ color: 'text.secondary' }}>
        &nbsp; {notification.notificationType}
      </Typography>
    </Typography>
  );

  let avatar;
  switch (notification.notificationType) {
    case 'Bilgilendirme':
      avatar = <InfoIcon />;
      break;
    case 'Uyarı':
      avatar = <WarningIcon />;
      break;
    case 'Hata':
      avatar = <ErrorIcon />;
      break;
    default:
      avatar = <AvatarIcon />;
      break;
  }

  return {
    avatar,
    title,
  };
}
