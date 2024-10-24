import React, { useState } from 'react';
import {
  Container,
  Typography,
  Card,
  CardContent,
  Grid,
  Switch,
  FormControlLabel,
  Button,
  Box,
} from '@mui/material';

// Components
import Iconify from '../components/iconify';

// Hooks
import useAuth from '../hooks/useAuth';


const SettingsPage = () => {
  const { user } = useAuth();
  const [settings, setSettings] = useState({
    darkMode: false, // Mod kontrolü yapılmadığı için varsayılan değer false olarak ayarlandı
    emailNotifications: true,
    pushNotifications: true,
  });

  const handleToggleChange = (event) => {
    const { name, checked } = event.target;
    setSettings((prev) => ({ ...prev, [name]: checked }));
  };

  const handleSaveSettings = async () => {
    try {
    
      // Başarılı işlem mesajı gösterme
    } catch (error) {
      console.error('Failed to update settings:', error);
      // Hata mesajı gösterme
    }
  };

  return (
    <Container maxWidth="md">
      <Typography variant="h4" sx={{ mb: 5 }}>
        Settings
      </Typography>

      <Card>
        <CardContent>
          <Grid container spacing={3}>
            <Grid item xs={12}>
              <FormControlLabel
                control={
                  <Switch
                    checked={settings.darkMode}
                    onChange={handleToggleChange}
                    name="darkMode"
                  />
                }
                label="Dark Mode"
              />
            </Grid>

            <Grid item xs={12}>
              <FormControlLabel
                control={
                  <Switch
                    checked={settings.emailNotifications}
                    onChange={handleToggleChange}
                    name="emailNotifications"
                  />
                }
                label="Email Notifications"
              />
            </Grid>

            <Grid item xs={12}>
              <FormControlLabel
                control={
                  <Switch
                    checked={settings.pushNotifications}
                    onChange={handleToggleChange}
                    name="pushNotifications"
                  />
                }
                label="Push Notifications"
              />
            </Grid>

            <Grid item xs={12}>
              <Box sx={{ display: 'flex', justifyContent: 'flex-end' }}>
                <Button
                  variant="contained"
                  onClick={handleSaveSettings}
                  startIcon={<Iconify icon="eva:save-fill" />}
                >
                  Save Settings
                </Button>
              </Box>
            </Grid>
          </Grid>
        </CardContent>
      </Card>
    </Container>
  );
};

export default SettingsPage;
