import React, { useState, useEffect } from 'react';
import { Helmet } from 'react-helmet-async';
import {
  Container,
  Typography,
  Card,
  CardContent,
  Grid,
  TextField,
  Button,
  Avatar,
  Box,
  IconButton,
  CircularProgress,
} from '@mui/material';
import { useTheme } from '@mui/material/styles';
import { Add, Edit, Save, Delete } from '@mui/icons-material';

// Components
import useAuth from '../hooks/useAuth';
import useNotification from '../hooks/useNotification';

// API
import {
  getProfile,
  updateProfile,
  getMyResume,
  createEducation,
  updateEducation,
  deleteEducation,
  createWorkExperience,
  updateWorkExperience,
  deleteWorkExperience,
  createSkill,
  updateSkill,
  deleteSkill,
  createCertification,
  updateCertification,
  deleteCertification,
  createLanguage,
  updateLanguage,
  deleteLanguage,
} from '../services/api';

const formatDate = (dateString) => {
  if (!dateString) return '';
  return dateString.split('T')[0];
};

const ProfilePage = () => {
  const theme = useTheme();
  const { user, updateUser } = useAuth();
  const { addNotification } = useNotification();
  const [profile, setProfile] = useState(null);
  const [resume, setResume] = useState({
    educations: [],
    workExperiences: [],
    skills: [],
    certifications: [],
    languages: [],
    additionalInformation: '',
  });
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [isEditing, setIsEditing] = useState(false);

  useEffect(() => {
    fetchProfile();
    if (user.role !== 'Admin') {
      fetchResume();
    }
  }, [user.role]);

  const fetchProfile = async () => {
    try {
      setLoading(true);
      const response = await getProfile();
      setProfile(response.data);
    } catch (err) {
      setError('Profil alınamadı');
      console.error('Profil alınamadı:', err);
    } finally {
      setLoading(false);
    }
  };

  const fetchResume = async () => {
    try {
      const response = await getMyResume();
      setResume(response.data);
    } catch (err) {
      setError('Özgeçmiş alınamadı');
      console.error('Özgeçmiş alınamadı:', err);
    }
  };

  const handleInputChange = (event) => {
    const { name, value } = event.target;
    setProfile((prev) => ({ ...prev, [name]: value }));
  };

  const handleResumeChange = (event, index, section) => {
    const { name, value } = event.target;
    setResume((prev) => ({
      ...prev,
      [section]: prev[section].map((item, idx) =>
        idx === index ? { ...item, [name]: value } : item
      ),
    }));
  };

  const createEmptySectionItem = (section) => {
    switch (section) {
      case 'educations':
        return { schoolName: '', degree: '', startDate: '', endDate: '', fieldOfStudy: '' };
      case 'workExperiences':
        return { companyName: '', position: '', startDate: '', endDate: '', description: '' };
      case 'skills':
        return { name: '', proficiency: '' };
      case 'certifications':
        return { name: '', issuer: '', dateIssued: '' };
      case 'languages':
        return { name: '', proficiency: '' };
      default:
        return {};
    }
  };

  const handleAddNew = (section) => {
    setResume((prev) => ({
      ...prev,
      [section]: [...prev[section], createEmptySectionItem(section)],
    }));
  };

  const handleDeleteItem = (index, section) => {
    setResume((prev) => ({
      ...prev,
      [section]: prev[section].filter((_, idx) => idx !== index),
    }));
  };

  const handleSubmit = async (event) => {
    event.preventDefault();
    try {
      await updateProfile(profile);
      addNotification({ type: 'success', message: 'Profil başarıyla güncellendi' });
      updateUser(profile);

      const updateOperations = [];

      resume.educations.forEach((edu) => {
        if (edu.id) {
          updateOperations.push(updateEducation(profile.id, edu.id, edu));
        } else {
          updateOperations.push(createEducation(profile.id, edu));
        }
      });

      resume.workExperiences.forEach((exp) => {
        if (exp.id) {
          updateOperations.push(updateWorkExperience(profile.id, exp.id, exp));
        } else {
          updateOperations.push(createWorkExperience(profile.id, exp));
        }
      });

      resume.skills.forEach((skill) => {
        if (skill.id) {
          updateOperations.push(updateSkill(profile.id, skill.id, skill));
        } else {
          updateOperations.push(createSkill(profile.id, skill));
        }
      });

      resume.certifications.forEach((cert) => {
        if (cert.id) {
          updateOperations.push(updateCertification(profile.id, cert.id, cert));
        } else {
          updateOperations.push(createCertification(profile.id, cert));
        }
      });

      resume.languages.forEach((lang) => {
        if (lang.id) {
          updateOperations.push(updateLanguage(profile.id, lang.id, lang));
        } else {
          updateOperations.push(createLanguage(profile.id, lang));
        }
      });

      await Promise.all(updateOperations);
      setIsEditing(false);
    } catch (err) {
      setError('Profil ve özgeçmiş güncellenemedi');
      console.error('Profil ve özgeçmiş güncellenemedi:', err);
      addNotification({ type: 'error', message: 'Profil ve özgeçmiş güncellenemedi' });
    }
  };

  const handleEditClick = () => {
    setIsEditing(true);
  };

  if (loading) {
    return (
      <Box sx={{ display: 'flex', justifyContent: 'center', alignItems: 'center', minHeight: '100vh' }}>
        <CircularProgress />
      </Box>
    );
  }

  if (error) {
    return <Typography color="error">{error}</Typography>;
  }

  return (
    <>
      <Helmet>
        <title>Profilim | İKYS</title>
      </Helmet>
      <Container maxWidth="md" sx={{ mt: 4 }}>
        <Typography variant="h4" sx={{ mb: 5, fontWeight: 'bold', color: theme.palette.primary.main }}>
          Profilim
        </Typography>

        <Card sx={{ mb: 5, p: 3 }}>
          <CardContent>
            <Box sx={{ display: 'flex', justifyContent: 'center', mb: 3 }}>
              <Avatar src={profile?.photoURL} alt={profile?.fullName} sx={{ width: 100, height: 100 }} />
            </Box>

            <form onSubmit={handleSubmit}>
              <Grid container spacing={3}>
                <Grid item xs={12} sm={6}>
                  <TextField
                    fullWidth
                    label="Ad"
                    name="firstName"
                    value={profile?.firstName || ''}
                    onChange={handleInputChange}
                    disabled={!isEditing}
                  />
                </Grid>
                <Grid item xs={12} sm={6}>
                  <TextField
                    fullWidth
                    label="Soyad"
                    name="lastName"
                    value={profile?.lastName || ''}
                    onChange={handleInputChange}
                    disabled={!isEditing}
                  />
                </Grid>
                <Grid item xs={12}>
                  <TextField
                    fullWidth
                    label="E-posta"
                    name="email"
                    value={profile?.email || ''}
                    onChange={handleInputChange}
                    disabled={!isEditing}
                  />
                </Grid>
                <Grid item xs={12}>
                  <TextField
                    fullWidth
                    label="Pozisyon"
                    name="position"
                    value={profile?.position || ''}
                    onChange={handleInputChange}
                    disabled={!isEditing}
                  />
                </Grid>
                <Grid item xs={12}>
                  <TextField
                    fullWidth
                    label="Telefon Numarası"
                    name="phoneNumber"
                    value={profile?.phoneNumber || ''}
                    onChange={handleInputChange}
                    disabled={!isEditing}
                  />
                </Grid>

                {user.role !== 'Admin' && resume && (
                  <>
                    {/* Eğitim Bilgileri */}
                    <Grid item xs={12}>
                      <Typography variant="h6" sx={{ mb: 2, fontWeight: 'bold' }}>
                        Eğitimler
                      </Typography>
                      {resume.educations.map((edu, index) => (
                        <Box key={index} sx={{ mb: 2 }}>
                          <Grid container spacing={2}>
                            <Grid item xs={12} sm={6}>
                              <TextField
                                fullWidth
                                label="Okul Adı"
                                name="schoolName"
                                value={edu.schoolName}
                                onChange={(e) => handleResumeChange(e, index, 'educations')}
                                disabled={!isEditing}
                              />
                            </Grid>
                            <Grid item xs={12} sm={6}>
                              <TextField
                                fullWidth
                                label="Derece"
                                name="degree"
                                value={edu.degree}
                                onChange={(e) => handleResumeChange(e, index, 'educations')}
                                disabled={!isEditing}
                              />
                            </Grid>
                            <Grid item xs={12} sm={6}>
                              <TextField
                                fullWidth
                                label="Başlangıç Tarihi"
                                name="startDate"
                                type="date"
                                value={formatDate(edu.startDate)}
                                onChange={(e) => handleResumeChange(e, index, 'educations')}
                                disabled={!isEditing}
                                InputLabelProps={{ shrink: true }}
                              />
                            </Grid>
                            <Grid item xs={12} sm={6}>
                              <TextField
                                fullWidth
                                label="Bitiş Tarihi"
                                name="endDate"
                                type="date"
                                value={formatDate(edu.endDate)}
                                onChange={(e) => handleResumeChange(e, index, 'educations')}
                                disabled={!isEditing}
                                InputLabelProps={{ shrink: true }}
                              />
                            </Grid>
                            <Grid item xs={12}>
                              <TextField
                                fullWidth
                                label="Bölüm"
                                name="fieldOfStudy"
                                value={edu.fieldOfStudy}
                                onChange={(e) => handleResumeChange(e, index, 'educations')}
                                disabled={!isEditing}
                              />
                            </Grid>
                            {isEditing && (
                              <Grid item xs={12} sm={2}>
                                <IconButton onClick={() => handleDeleteItem(index, 'educations')}>
                                  <Delete />
                                </IconButton>
                              </Grid>
                            )}
                          </Grid>
                        </Box>
                      ))}
                      {isEditing && (
                        <Button variant="outlined" onClick={() => handleAddNew('educations')}>
                          Eğitim Ekle
                        </Button>
                      )}
                    </Grid>

                    {/* İş Deneyimi */}
                    <Grid item xs={12}>
                      <Typography variant="h6" sx={{ mb: 2, fontWeight: 'bold' }}>
                        İş Deneyimleri
                      </Typography>
                      {resume.workExperiences.map((exp, index) => (
                        <Box key={index} sx={{ mb: 2 }}>
                          <Grid container spacing={2}>
                            <Grid item xs={12} sm={6}>
                              <TextField
                                fullWidth
                                label="Şirket Adı"
                                name="companyName"
                                value={exp.companyName}
                                onChange={(e) => handleResumeChange(e, index, 'workExperiences')}
                                disabled={!isEditing}
                              />
                            </Grid>
                            <Grid item xs={12} sm={6}>
                              <TextField
                                fullWidth
                                label="Pozisyon"
                                name="position"
                                value={exp.position}
                                onChange={(e) => handleResumeChange(e, index, 'workExperiences')}
                                disabled={!isEditing}
                              />
                            </Grid>
                            <Grid item xs={12} sm={6}>
                              <TextField
                                fullWidth
                                label="Başlangıç Tarihi"
                                name="startDate"
                                type="date"
                                value={formatDate(exp.startDate)}
                                onChange={(e) => handleResumeChange(e, index, 'workExperiences')}
                                disabled={!isEditing}
                                InputLabelProps={{ shrink: true }}
                              />
                            </Grid>
                            <Grid item xs={12} sm={6}>
                              <TextField
                                fullWidth
                                label="Bitiş Tarihi"
                                name="endDate"
                                type="date"
                                value={formatDate(exp.endDate)}
                                onChange={(e) => handleResumeChange(e, index, 'workExperiences')}
                                disabled={!isEditing}
                                InputLabelProps={{ shrink: true }}
                              />
                            </Grid>
                            <Grid item xs={12}>
                              <TextField
                                fullWidth
                                label="Açıklama"
                                name="description"
                                value={exp.description}
                                onChange={(e) => handleResumeChange(e, index, 'workExperiences')}
                                disabled={!isEditing}
                              />
                            </Grid>
                            {isEditing && (
                              <Grid item xs={12} sm={2}>
                                <IconButton onClick={() => handleDeleteItem(index, 'workExperiences')}>
                                  <Delete />
                                </IconButton>
                              </Grid>
                            )}
                          </Grid>
                        </Box>
                      ))}
                      {isEditing && (
                        <Button variant="outlined" onClick={() => handleAddNew('workExperiences')}>
                          İş Deneyimi Ekle
                        </Button>
                      )}
                    </Grid>

                    {/* Yetenekler */}
                    <Grid item xs={12}>
                      <Typography variant="h6" sx={{ mb: 2, fontWeight: 'bold' }}>
                        Yetenekler
                      </Typography>
                      {resume.skills.map((skill, index) => (
                        <Box key={index} sx={{ mb: 2 }}>
                          <Grid container spacing={2}>
                            <Grid item xs={12} sm={10}>
                              <TextField
                                fullWidth
                                label="Yetenek"
                                name="name"
                                value={skill.name}
                                onChange={(e) => handleResumeChange(e, index, 'skills')}
                                disabled={!isEditing}
                              />
                            </Grid>
                            <Grid item xs={12} sm={10}>
                              <TextField
                                fullWidth
                                label="Yeterlilik"
                                name="proficiency"
                                value={skill.proficiency}
                                onChange={(e) => handleResumeChange(e, index, 'skills')}
                                disabled={!isEditing}
                              />
                            </Grid>
                            {isEditing && (
                              <Grid item xs={12} sm={2}>
                                <IconButton onClick={() => handleDeleteItem(index, 'skills')}>
                                  <Delete />
                                </IconButton>
                              </Grid>
                            )}
                          </Grid>
                        </Box>
                      ))}
                      {isEditing && (
                        <Button variant="outlined" onClick={() => handleAddNew('skills')}>
                          Yetenek Ekle
                        </Button>
                      )}
                    </Grid>

                    {/* Sertifikalar */}
                    <Grid item xs={12}>
                      <Typography variant="h6" sx={{ mb: 2, fontWeight: 'bold' }}>
                        Sertifikalar
                      </Typography>
                      {resume.certifications.map((cert, index) => (
                        <Box key={index} sx={{ mb: 2 }}>
                          <Grid container spacing={2}>
                            <Grid item xs={12} sm={6}>
                              <TextField
                                fullWidth
                                label="Sertifika Adı"
                                name="name"
                                value={cert.name}
                                onChange={(e) => handleResumeChange(e, index, 'certifications')}
                                disabled={!isEditing}
                              />
                            </Grid>
                            <Grid item xs={12} sm={6}>
                              <TextField
                                fullWidth
                                label="Veren Kurum"
                                name="issuer"
                                value={cert.issuer}
                                onChange={(e) => handleResumeChange(e, index, 'certifications')}
                                disabled={!isEditing}
                              />
                            </Grid>
                            <Grid item xs={12} sm={6}>
                              <TextField
                                fullWidth
                                label="Veriliş Tarihi"
                                name="dateIssued"
                                type="date"
                                value={formatDate(cert.dateIssued)}
                                onChange={(e) => handleResumeChange(e, index, 'certifications')}
                                disabled={!isEditing}
                                InputLabelProps={{ shrink: true }}
                              />
                            </Grid>
                            {isEditing && (
                              <Grid item xs={12} sm={2}>
                                <IconButton onClick={() => handleDeleteItem(index, 'certifications')}>
                                  <Delete />
                                </IconButton>
                              </Grid>
                            )}
                          </Grid>
                        </Box>
                      ))}
                      {isEditing && (
                        <Button variant="outlined" onClick={() => handleAddNew('certifications')}>
                          Sertifika Ekle
                        </Button>
                      )}
                    </Grid>

                    {/* Diller */}
                    <Grid item xs={12}>
                      <Typography variant="h6" sx={{ mb: 2, fontWeight: 'bold' }}>
                        Diller
                      </Typography>
                      {resume.languages.map((lang, index) => (
                        <Box key={index} sx={{ mb: 2 }}>
                          <Grid container spacing={2}>
                            <Grid item xs={12} sm={6}>
                              <TextField
                                fullWidth
                                label="Dil"
                                name="name"
                                value={lang.name}
                                onChange={(e) => handleResumeChange(e, index, 'languages')}
                                disabled={!isEditing}
                              />
                            </Grid>
                            <Grid item xs={12} sm={6}>
                              <TextField
                                fullWidth
                                label="Yeterlilik"
                                name="proficiency"
                                value={lang.proficiency}
                                onChange={(e) => handleResumeChange(e, index, 'languages')}
                                disabled={!isEditing}
                              />
                            </Grid>
                            {isEditing && (
                              <Grid item xs={12} sm={2}>
                                <IconButton onClick={() => handleDeleteItem(index, 'languages')}>
                                  <Delete />
                                </IconButton>
                              </Grid>
                            )}
                          </Grid>
                        </Box>
                      ))}
                      {isEditing && (
                        <Button variant="outlined" onClick={() => handleAddNew('languages')}>
                          Dil Ekle
                        </Button>
                      )}
                    </Grid>
                  </>
                )}

                {/* Profil ve Özgeçmiş Güncelleme Butonları */}
                <Grid item xs={12}>
                  {isEditing ? (
                    <Button
                      type="submit"
                      variant="contained"
                      startIcon={<Save />}
                      sx={{ backgroundColor: theme.palette.success.main }}
                    >
                      Değişiklikleri Kaydet
                    </Button>
                  ) : (
                    <Button
                      variant="contained"
                      onClick={handleEditClick}
                      startIcon={<Edit />}
                      sx={{ backgroundColor: theme.palette.info.main }}
                    >
                      Profili Düzenle
                    </Button>
                  )}
                </Grid>
              </Grid>
            </form>
          </CardContent>
        </Card>
      </Container>
    </>
  );
};

export default ProfilePage;
