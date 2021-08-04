using System;
using FluentAssertions;
using System.Collections.Generic;
using System.Text;
using Xunit;
using System.Linq;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace RandomFixtureKit.Tests
{
    public class TreeStructureTest
    {
        [Fact]
        public void CheckArray()
        {
            FixtureFactory.Create<ArrayTreeNode>();
        }

        [Fact]
        public void CheckArraySegment() {
            FixtureFactory.Create<ArraySegmentTreeNode>();
        }

        [Fact]
        public void CheckList() {
            FixtureFactory.Create<ListTreeNode>();
        }

        [Fact]
        public void CheckLinkedList() {
            FixtureFactory.Create<LinkedListTreeNode>();
        }

        [Fact]
        public void CheckStack() {
            FixtureFactory.Create<StackTreeNode>();
        }

        [Fact]
        public void CheckQueue() {
            FixtureFactory.Create<QueueTreeNode>();
        }

        [Fact]
        public void CheckConcurrentQueue() {
            FixtureFactory.Create<ConcurrentQueueTreeNode>();
        }

        [Fact]
        public void CheckConcurrentStack() {
            FixtureFactory.Create<ConcurrentStackTreeNode>();
        }

        [Fact]
        public void CheckICollection() {
            FixtureFactory.Create<ICollectionTreeNode>();
        }

        [Fact]
        public void CheckIEnumerable() {
            FixtureFactory.Create<IEnumerableTreeNode>();
        }

        [Fact]
        public void CheckIList() {
            FixtureFactory.Create<IListTreeNode>();
        }

        [Fact]
        public void CheckIReadOnlyCollection() {
            FixtureFactory.Create<IReadOnlyCollectionTreeNode>();
        }

        [Fact]
        public void CheckIReadOnlyList() {
            FixtureFactory.Create<IReadOnlyListTreeNode>();
        }

        [Fact]
        public void CheckDictionary() {
            FixtureFactory.Create<DictionaryTreeNode>();
        }

        [Fact]
        public void CheckSortedDictionary() {
            FixtureFactory.Create<SortedDictionaryTreeNode>();
        }

        [Fact]
        public void CheckSortedList() {
            FixtureFactory.Create<SortedListTreeNode>();
        }

        [Fact]
        public void CheckIDictionary() {
            FixtureFactory.Create<IDictionaryTreeNode>();
        }

        [Fact]
        public void CheckIReadOnlyDictionary() {
            FixtureFactory.Create<IReadOnlyDictionaryTreeNode>();
        }

        }


    class ArrayTreeNode
    {
        public string Name { get; set; }
        public ArrayTreeNode[] Children { get; set; }
    }

    
    class ArraySegmentTreeNode {
        public string Name { get; set; }
        public ArraySegment<ArraySegmentTreeNode> Children { get; set; }
    }
    
    class ListTreeNode {
        public string Name { get; set; }
        public List<ListTreeNode> Children { get; set; }
    }
    
    class LinkedListTreeNode {
        public string Name { get; set; }
        public LinkedList<LinkedListTreeNode> Children { get; set; }
    }
    
    class StackTreeNode {
        public string Name { get; set; }
        public Stack<StackTreeNode> Children { get; set; }
    }
    
    class QueueTreeNode {
        public string Name { get; set; }
        public Queue<QueueTreeNode> Children { get; set; }
    }
    
    class ConcurrentQueueTreeNode {
        public string Name { get; set; }
        public ConcurrentQueue<ConcurrentQueueTreeNode> Children { get; set; }
    }
    
    class ConcurrentStackTreeNode {
        public string Name { get; set; }
        public ConcurrentStack<ConcurrentStackTreeNode> Children { get; set; }
    }
    
    class ICollectionTreeNode {
        public string Name { get; set; }
        public ICollection<ICollectionTreeNode> Children { get; set; }
    }
    
    class IEnumerableTreeNode {
        public string Name { get; set; }
        public IEnumerable<IEnumerableTreeNode> Children { get; set; }
    }
    
    class IListTreeNode {
        public string Name { get; set; }
        public IList<IListTreeNode> Children { get; set; }
    }
    
    class IReadOnlyCollectionTreeNode {
        public string Name { get; set; }
        public IReadOnlyCollection<IReadOnlyCollectionTreeNode> Children { get; set; }
    }
    
    class IReadOnlyListTreeNode {
        public string Name { get; set; }
        public IReadOnlyList<IReadOnlyListTreeNode> Children { get; set; }
    }
    
    
    class DictionaryTreeNode {
        public string Name { get; set; }
        public Dictionary<string, DictionaryTreeNode> Children { get; set; }
    }
    
    class SortedDictionaryTreeNode {
        public string Name { get; set; }
        public SortedDictionary<string, SortedDictionaryTreeNode> Children { get; set; }
    }
    
    class SortedListTreeNode {
        public string Name { get; set; }
        public SortedList<string, SortedListTreeNode> Children { get; set; }
    }
    
    class IDictionaryTreeNode {
        public string Name { get; set; }
        public IDictionary<string, IDictionaryTreeNode> Children { get; set; }
    }
    
    class IReadOnlyDictionaryTreeNode {
        public string Name { get; set; }
        public IReadOnlyDictionary<string, IReadOnlyDictionaryTreeNode> Children { get; set; }
    }
    }
