import React, { useState } from 'react';
import {
  Card,
  CardContent,
  Typography,
  Switch,
  FormControlLabel,
  Button,
  Box,
} from '@mui/material';
import useNotification from '../../hooks/useNotification';

const NotificationSettings = () => {
  const [settings, setSettings] = useState({
    emailNotifications: false,
    pushNotifications: false,
    smsNotifications: false,
    newLeaveRequests: false,
    leaveStatusUpdates: false,
    newExpenseReports: false,
    shiftAssignments: false,
  });
  const { addNotification } = useNotification();

  // API çağrıları devre dışı bırakıldı

  const handleToggle = (setting) => {
    setSettings((prevSettings) => ({
      ...prevSettings,
      [setting]: !prevSettings[setting],
    }));
  };

  const handleSave = () => {
    // API çağrısı yerine geçici bir işlem
    addNotification({ type: 'success', message: 'Notification settings updated successfully (Simulated)' });
  };

  return (
    <Card>
      <CardContent>
        <Typography variant="h4" gutterBottom>Notification Settings</Typography>
        <Box mb={3}>
          <Typography variant="h6" gutterBottom>Notification Channels</Typography>
          <FormControlLabel
            control={<Switch checked={settings.emailNotifications} onChange={() => handleToggle('emailNotifications')} />}
            label="Email Notifications"
          />
          <FormControlLabel
            control={<Switch checked={settings.pushNotifications} onChange={() => handleToggle('pushNotifications')} />}
            label="Push Notifications"
          />
          <FormControlLabel
            control={<Switch checked={settings.smsNotifications} onChange={() => handleToggle('smsNotifications')} />}
            label="SMS Notifications"
          />
        </Box>
        <Box mb={3}>
          <Typography variant="h6" gutterBottom>Notification Types</Typography>
          <FormControlLabel
            control={<Switch checked={settings.newLeaveRequests} onChange={() => handleToggle('newLeaveRequests')} />}
            label="New Leave Requests"
          />
          <FormControlLabel
            control={<Switch checked={settings.leaveStatusUpdates} onChange={() => handleToggle('leaveStatusUpdates')} />}
            label="Leave Status Updates"
          />
          <FormControlLabel
            control={<Switch checked={settings.newExpenseReports} onChange={() => handleToggle('newExpenseReports')} />}
            label="New Expense Reports"
          />
          <FormControlLabel
            control={<Switch checked={settings.shiftAssignments} onChange={() => handleToggle('shiftAssignments')} />}
            label="Shift Assignments"
          />
        </Box>
        <Button variant="contained" color="primary" onClick={handleSave}>
          Save Settings
        </Button>
      </CardContent>
    </Card>
  );
};

export default NotificationSettings;
