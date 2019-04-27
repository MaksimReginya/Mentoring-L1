use library

// 1
db.books.insertMany([
	{"name": "Hobbit", "author": "Tolkien", "count": 5, genre: ["fantasy"], "year": 2014},
	{"name": "Lord of the rings", "author": "Tolkien", "count": 3, genre: ["fantasy"], "year": 2015},
	{"name": "Kolobok", "count": 10, genre: ["kids"], "year": 2000},
	{"name": "Repka", "count": 11, genre: ["kids"], "year": 2000},
	{"name": "Dyadya Stiopa", "author": "Mihalkov", "count": 1, genre: ["kids"], "year": 2001}
])

// 2 (a, b, c)
db.books.find({count: {$gt: 1}}, {name: 1, _id: 0}).sort({name: 1}).limit(3)

// 2 (d)
db.books.find({count: {$gt: 1}}, {name: 1, _id: 0}).sort({name: 1}).count()

// 3
db.books.find().sort({count: -1}).limit(1).pretty()
db.books.find().sort({count: 1}).limit(1).pretty()

// 4
db.books.distinct("author")

// 5
db.books.find({}, {author: 0}).pretty()

// 6
db.books.updateMany({}, {$inc: {count: 1}})

// 7
db.books.updateMany({genre: "fantasy"}, {$addToSet: {genre: "favority"}})

// 8
db.books.remove({count: {$lt: 3}})

// 9
db.books.drop()
