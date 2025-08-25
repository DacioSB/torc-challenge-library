import React, { useState, useEffect } from 'react';
import { TextField, Button, Select, MenuItem, FormControl, InputLabel, Table, TableBody, TableCell, TableContainer, TableHead, TableRow, Paper, Typography, Container, Grid, Checkbox, FormGroup, FormControlLabel, Pagination, Box, TableSortLabel } from '@mui/material';

const API_BASE_URL = process.env.REACT_APP_API_URL || '${API_BASE_URL}';

interface PagedResult<T> {
  data: T[];
  totalCount: number;
  page: number;
  pageSize: number;
  totalPages: number;
  hasNextPage: boolean;
  hasPreviousPage: boolean;
}

function App() {
  const [searchBy, setSearchBy] = useState('author');
  const [searchValue, setSearchValue] = useState('');
  const [books, setBooks] = useState<any[]>([]);
  const [selectedOwnerships, setSelectedOwnerships] = useState<string[]>([]);
  const [currentPage, setCurrentPage] = useState(1);
  const [totalPages, setTotalPages] = useState(1);
  const [totalCount, setTotalCount] = useState(0);
  const [sortBy, setSortBy] = useState<string>('');
  const [sortDirection, setSortDirection] = useState<'asc' | 'desc'>('asc');
  const pageSize = 5;

  const ownershipMap: { [key: number]: string } = {
    0: 'Own',
    1: 'Love',
    2: 'WantToRead',
  };

  // Helper function to sort books array
  const sortBooks = (booksArray: any[], column: string, direction: 'asc' | 'desc') => {
    return [...booksArray].sort((a, b) => {
      let aValue: any;
      let bValue: any;
      
      switch (column) {
        case 'title':
          aValue = a.title || '';
          bValue = b.title || '';
          break;
        case 'author':
          aValue = `${a.firstName || ''} ${a.lastName || ''}`.trim();
          bValue = `${b.firstName || ''} ${b.lastName || ''}`.trim();
          break;
        case 'isbn':
          aValue = a.isbn || '';
          bValue = b.isbn || '';
          break;
        case 'category':
          aValue = a.category || '';
          bValue = b.category || '';
          break;
        default:
          return 0;
      }
      
      // Convert to lowercase for case-insensitive sorting
      if (typeof aValue === 'string') aValue = aValue.toLowerCase();
      if (typeof bValue === 'string') bValue = bValue.toLowerCase();
      
      if (aValue < bValue) {
        return direction === 'asc' ? -1 : 1;
      }
      if (aValue > bValue) {
        return direction === 'asc' ? 1 : -1;
      }
      return 0;
    });
  };

  const fetchBooks = async (page: number = 1, sort?: string, direction?: 'asc' | 'desc') => {
    let url = `${API_BASE_URL}/api/books?page=${page}&pageSize=${pageSize}`;
    if (sort) {
      url += `&sortBy=${sort}&sortDirection=${direction || 'asc'}`;
    }
    const response = await fetch(url);
    const data: PagedResult<any> = await response.json();
    setBooks(data.data);
    setCurrentPage(data.page);
    setTotalPages(data.totalPages);
    setTotalCount(data.totalCount);
  };

  useEffect(() => {
    fetchBooks(1);
  }, []);

  const handleSearch = async () => {
    let queryValue = searchValue;
    if (searchBy === 'ownership') {
      queryValue = selectedOwnerships.join(',');
    }
    let url = `${API_BASE_URL}/api/books?searchBy=${searchBy}&searchValue=${queryValue}&page=1&pageSize=${pageSize}`;
    if (sortBy) {
      url += `&sortBy=${sortBy}&sortDirection=${sortDirection}`;
    }
    const response = await fetch(url);
    const data: PagedResult<any> = await response.json();
    
    // Apply frontend sorting if there's an active sort
    let newBooks = data.data;
    if (sortBy) {
      newBooks = sortBooks(data.data, sortBy, sortDirection);
    }
    
    setBooks(newBooks);
    setCurrentPage(data.page);
    setTotalPages(data.totalPages);
    setTotalCount(data.totalCount);
  };

  const handleClear = () => {
    setSearchValue('');
    setSelectedOwnerships([]);
    setSortBy('');
    setSortDirection('asc');
    setCurrentPage(1);
    fetchBooks(1);
  };

  const handlePageChange = async (event: React.ChangeEvent<unknown>, page: number) => {
    let queryValue = searchValue;
    if (searchBy === 'ownership') {
      queryValue = selectedOwnerships.join(',');
    }
    
    let url = `${API_BASE_URL}/api/books?page=${page}&pageSize=${pageSize}`;
    if (searchBy && queryValue) {
      url += `&searchBy=${searchBy}&searchValue=${queryValue}`;
    }
    if (sortBy) {
      url += `&sortBy=${sortBy}&sortDirection=${sortDirection}`;
    }
    
    const response = await fetch(url);
    const data: PagedResult<any> = await response.json();
    
    // Apply frontend sorting if there's an active sort
    let newBooks = data.data;
    if (sortBy) {
      newBooks = sortBooks(data.data, sortBy, sortDirection);
    }
    
    setBooks(newBooks);
    setCurrentPage(data.page);
    setTotalPages(data.totalPages);
    setTotalCount(data.totalCount);
  };

  const handleSort = (column: string) => {
    const isAsc = sortBy === column && sortDirection === 'asc';
    const newDirection: 'asc' | 'desc' = isAsc ? 'desc' : 'asc';
    
    setSortBy(column);
    setSortDirection(newDirection);
    
    // Sort the current books array on the frontend using helper function
    const sortedBooks = sortBooks(books, column, newDirection);
    setBooks(sortedBooks);
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
              <MenuItem value="ownership">Ownership</MenuItem>
            </Select>
          </FormControl>
        </Grid>
        <Grid size={{ xs: 12, sm: 4 }}>
          {searchBy === 'ownership' ? (
            <FormControl component="fieldset" fullWidth>
              <FormGroup row>
                {Object.entries(ownershipMap).map(([key, value]) => (
                  <FormControlLabel
                    key={key}
                    control={
                      <Checkbox
                        checked={selectedOwnerships.includes(value)}
                        onChange={(e) => {
                          if (e.target.checked) {
                            setSelectedOwnerships([...selectedOwnerships, value]);
                          } else {
                            setSelectedOwnerships(selectedOwnerships.filter((item) => item !== value));
                          }
                        }}
                        name={value}
                      />
                    }
                    label={value}
                  />
                ))}
              </FormGroup>
            </FormControl>
          ) : (
            <TextField fullWidth label="Search Value" value={searchValue} onChange={(e) => setSearchValue(e.target.value)} />
          )}
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
              <TableCell>
                <TableSortLabel
                  active={sortBy === 'title'}
                  direction={sortBy === 'title' ? sortDirection : 'asc'}
                  onClick={() => handleSort('title')}
                >
                  Title
                </TableSortLabel>
              </TableCell>
              <TableCell>
                <TableSortLabel
                  active={sortBy === 'author'}
                  direction={sortBy === 'author' ? sortDirection : 'asc'}
                  onClick={() => handleSort('author')}
                >
                  Author
                </TableSortLabel>
              </TableCell>
              <TableCell>
                <TableSortLabel
                  active={sortBy === 'isbn'}
                  direction={sortBy === 'isbn' ? sortDirection : 'asc'}
                  onClick={() => handleSort('isbn')}
                >
                  ISBN
                </TableSortLabel>
              </TableCell>
              <TableCell>
                <TableSortLabel
                  active={sortBy === 'category'}
                  direction={sortBy === 'category' ? sortDirection : 'asc'}
                  onClick={() => handleSort('category')}
                >
                  Category
                </TableSortLabel>
              </TableCell>
              <TableCell>Ownership</TableCell>
            </TableRow>
          </TableHead>
          <TableBody>
            {books.map((book: any) => (
              <TableRow key={book.bookId} hover>
                <TableCell>{book.title}</TableCell>
                <TableCell>{`${book.firstName} ${book.lastName}`}</TableCell>
                <TableCell>{book.isbn}</TableCell>
                <TableCell>{book.category}</TableCell>
                <TableCell>{ownershipMap[book.ownership]}</TableCell>
              </TableRow>
            ))}
          </TableBody>
        </Table>
      </TableContainer>

      {/* Pagination Controls */}
      <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', mt: 3 }}>
        <Typography variant="body2" color="text.secondary">
          Showing {books.length} of {totalCount} books
        </Typography>
        <Pagination
          count={totalPages}
          page={currentPage}
          onChange={handlePageChange}
          color="primary"
          showFirstButton
          showLastButton
        />
      </Box>
    </Container>
  );
}

export default App;
