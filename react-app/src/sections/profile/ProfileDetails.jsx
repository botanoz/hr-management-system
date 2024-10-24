import React, { useState, useEffect, useCallback } from 'react';
import {
  Card,
  CardContent,
  Typography,
  Grid,
  Avatar,
  Button,
  Chip,
} from '@mui/material';
import { Edit as EditIcon } from '@mui/icons-material';
import { getProfile } from '../../services/api';
import useAuth from '../../hooks/useAuth';
import useNotification from '../../hooks/useNotification';
import { fDate } from '../../utils/formatTime';
import ProfileForm from './ProfileForm';

const ProfileDetails = () => {
  const [profile, setProfile] = useState(null);
  const [isEditing, setIsEditing] = useState(false);
  const { user } = useAuth();
  const { addNotification } = useNotification();

  const fetchProfileDetails = useCallback(async () => {
    try {
      const response = await getProfile();
      setProfile(response.data);
    } catch (error) {
      addNotification({ type: 'error', message: 'Failed to fetch profile details' });
    }
  }, [addNotification]);

  useEffect(() => {
    fetchProfileDetails();
  }, [fetchProfileDetails]);

  const handleEditClick = () => {
    setIsEditing(true);
  };

  const handleFormClose = () => {
    setIsEditing(false);
    fetchProfileDetails();
  };

  if (!profile) {
    return <Typography>Loading...</Typography>;
  }

  return (
    <Card>
      <CardContent>
        <Grid container spacing={3} alignItems="center">
          <Grid item xs={12} sm={4} md={3}>
            <Avatar
              alt={profile.name}
              src={profile.avatar}
              sx={{ width: 150, height: 150, margin: 'auto' }}
            />
          </Grid>
          <Grid item xs={12} sm={8} md={9}>
            <Typography variant="h4" gutterBottom>
              {profile.name}
              <Button
                startIcon={<EditIcon />}
                onClick={handleEditClick}
                sx={{ ml: 2 }}
              >
                Edit Profile
              </Button>
            </Typography>
            <Typography variant="subtitle1" color="textSecondary" gutterBottom>
              {profile.jobTitle}
            </Typography>
            <Chip label={profile.department} color="primary" sx={{ mt: 1, mb: 2 }} />
          </Grid>
        </Grid>
        <Grid container spacing={2} sx={{ mt: 2 }}>
          <Grid item xs={12} sm={6}>
            <Typography variant="body1">
              <strong>Email:</strong> {profile.email}
            </Typography>
          </Grid>
          <Grid item xs={12} sm={6}>
            <Typography variant="body1">
              <strong>Phone:</strong> {profile.phone}
            </Typography>
          </Grid>
          <Grid item xs={12} sm={6}>
            <Typography variant="body1">
              <strong>Date of Birth:</strong> {fDate(profile.dateOfBirth)}
            </Typography>
          </Grid>
          <Grid item xs={12} sm={6}>
            <Typography variant="body1">
              <strong>Joined Date:</strong> {fDate(profile.joinedDate)}
            </Typography>
          </Grid>
          <Grid item xs={12}>
            <Typography variant="body1">
              <strong>Address:</strong> {profile.address}
            </Typography>
          </Grid>
          {profile.bio && (
            <Grid item xs={12}>
              <Typography variant="body1">
                <strong>Bio:</strong> {profile.bio}
              </Typography>
            </Grid>
          )}
        </Grid>
      </CardContent>
      <ProfileForm
        open={isEditing}
        onClose={handleFormClose}
        profile={profile}
      />
    </Card>
  );
};

export default ProfileDetails;
