import React, { useState, useEffect } from 'react';
import { TextField, Button, Select, MenuItem, FormControl, InputLabel, Table, TableBody, TableCell, TableContainer, TableHead, TableRow, Paper, Typography, Container, Grid } from '@mui/material';

function App() {
  const [searchBy, setSearchBy] = useState('author');
  const [searchValue, setSearchValue] = useState('');
  const [books, setBooks] = useState<any[]>([]);

  const fetchBooks = async () => {
    const response = await fetch('http://localhost:5043/Books');
    const data = await response.json();
    setBooks(data);
  };

  useEffect(() => {
    fetchBooks();
  }, []);

  const handleSearch = async () => {
    const response = await fetch(`http://localhost:5043/Books?searchBy=${searchBy}&searchValue=${searchValue}`);
    const data = await response.json();
    setBooks(data);
  };

  const handleClear = () => {
    setSearchValue('');
    fetchBooks();
  };

  return (
    <Container maxWidth="lg">
      <Typography variant="h2" component="h1" gutterBottom sx={{ mt: 4, mb: 4, textAlign: 'center' }}>
        Royal Library
      </Typography>
      <Grid container spacing={2} alignItems="center" sx={{ mb: 4 }}>
        <Grid size={{ xs: 12, sm: 4 }}>
          <FormControl fullWidth>
            <InputLabel>Search By</InputLabel>
            <Select value={searchBy} onChange={(e) => setSearchBy(e.target.value)}>
              <MenuItem value="author">Author</MenuItem>
              <MenuItem value="isbn">ISBN</MenuItem>
              <MenuItem value="title">Title</MenuItem>
            </Select>
          </FormControl>
        </Grid>
        <Grid size={{ xs: 12, sm: 4 }}>
          <TextField fullWidth label="Search Value" value={searchValue} onChange={(e) => setSearchValue(e.target.value)} />
        </Grid>
        <Grid size={{ xs: 12, sm: 4 }}>
          <Button variant="contained" onClick={handleSearch} sx={{ mr: 2 }}>Search</Button>
          <Button variant="outlined" onClick={handleClear}>Clear</Button>
        </Grid>
      </Grid>

      <TableContainer component={Paper} elevation={3}>
        <Table sx={{ minWidth: 650 }}>
          <TableHead sx={{ backgroundColor: '#f5f5f5' }}>
            <TableRow>
              <TableCell>Title</TableCell>
              <TableCell>Author</TableCell>
              <TableCell>ISBN</TableCell>
              <TableCell>Category</TableCell>
            </TableRow>
          </TableHead>
          <TableBody>
            {books.map((book: any) => (
              <TableRow key={book.bookId} hover>
                <TableCell>{book.title}</TableCell>
                <TableCell>{`${book.firstName} ${book.lastName}`}</TableCell>
                <TableCell>{book.isbn}</TableCell>
                <TableCell>{book.category}</TableCell>
              </TableRow>
            ))}
          </TableBody>
        </Table>
      </TableContainer>
    </Container>
  );
}

export default App;