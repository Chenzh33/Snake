using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class LinkNode<T> {
	public T data;
	public LinkNode<T> prev;
	public LinkNode<T> next;
	public LinkNode (T _data) {
		data = _data;
		prev = null;
		next = null;
	}

	public LinkNode (LinkNode<T> n) {
		data = n.data;
		prev = n.prev;
		next = n.next;
	}

}
public class LinkList<T> {
	public LinkNode<T> head;

	public LinkList () {
		head = null;
	}

	public void AddNodeEnd (LinkNode<T> n) {
		if (head == null) {
			head = n;
		} else {
			LinkNode<T> iter = head;
			while (iter.next != null)
				iter = iter.next;
			iter.next = n;
			n.prev = iter;

		}

	}

	public void AddNodeBegin (LinkNode<T> n) {
		if (head == null) {
			head = n;
		} else {
			head.prev = n;
			n.next = head;
			head = n;
		}

	}

	public LinkNode<T> FirstNode () {
		return head;
	}

	public LinkNode<T> LastNode () {
		LinkNode<T> p = head;
		if (p == null)
			return p;
		while (p.next != null) {
			p = p.next;
		}

		return p;
	}


	public void DeleteFirstNode () {
		if (head != null) {

			head = head.next;
			if (head != null)
				head.prev = null;

		}
	}

	public void PrintList () {
		LinkNode<T> p = head;
		int l = 0;
		while (p != null) {
			Debug.Log (l.ToString () + ": " + p.data.ToString ());
			p = p.next;
			l++;
		}
	}
}